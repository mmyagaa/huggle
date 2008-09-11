Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Web.HttpUtility

<DebuggerStepThrough()> _
Module Misc

    'Globals

    Public Administrator As Boolean
    Public AllLists As New Dictionary(Of String, List(Of String))
    Public AllRequests As New List(Of Request)
    Public Bots As New List(Of String)
    Public ContribsOffset As Integer
    Public Config As Configuration
    Public Cookie As String
    Public CurrentQueue As Queue
    Public CurrentTab As BrowserTab
    Public HidingEdit As Boolean = True
    Public HistoryOffset As Integer
    Public LastRcTime As Date
    Public LastTagText As String = ""
    Public LatestDiffRequest As DiffRequest
    Public LogBuffer As New List(Of String)
    Public MainForm As Main
    Public NextCount As New List(Of User)
    Public NullEdit As New Edit
    Public PendingRequests As New List(Of Request)
    Public PendingWarnings As New List(Of Edit)
    Public QueueNames As New Dictionary(Of String, List(Of String))
    Public RollbackAvailable As Boolean
    Public SpeedyCriteria As New Dictionary(Of String, SpeedyCriterion)
    Public StartTime As Date
    Public SyncContext As Threading.SynchronizationContext
    Public Undo As New List(Of Command)
    Public Watchlist As New List(Of Page)
    Public Whitelist As New List(Of String)
    Public WhitelistAutoChanges As New List(Of String)
    Public WhitelistLoaded As Boolean
    Public WhitelistManualChanges As New List(Of String)

    Public Delegate Sub Action()
    Public Delegate Sub CallbackDelegate(ByVal Success As Boolean)

    Public ReadOnly Tab As Char = Convert.ToChar(9)
    Public ReadOnly LF As Char = Convert.ToChar(10)
    Public ReadOnly CR As Char = Convert.ToChar(13)
    Public ReadOnly CRLF As String = Convert.ToChar(13) & Convert.ToChar(10)

    Public Property CurrentEdit() As Edit
        Get
            If CurrentTab Is Nothing Then CurrentTab = CType(MainForm.Tabs.TabPages(0).Controls(0), BrowserTab)
            Return CurrentTab.Edit
        End Get

        Set(ByVal value As Edit)
            If CurrentTab IsNot Nothing Then CurrentTab.Edit = value
        End Set
    End Property

    Public ReadOnly Property CurrentUser() As User
        Get
            If CurrentEdit Is Nothing Then Return Nothing Else Return CurrentEdit.User
        End Get
    End Property

    Public ReadOnly Property CurrentPage() As Page
        Get
            If CurrentEdit Is Nothing Then Return Nothing Else Return CurrentEdit.Page
        End Get
    End Property

    Class Block
        Public Time As Date
        Public User As User
        Public Action As String
        Public Duration As String
        Public Options As String
        Public Admin As User
        Public Comment As String
    End Class

    Class CacheData
        Public Edit As Edit
        Public Text As String
    End Class

    Class Command
        Public Description As String
        Public Type As CommandType
        Public Edit As Edit
        Public Page As Page
        Public User As User
    End Class

    Public Enum CommandType As Integer
        Revert
        Report
        Warning
        Edit
        Ignore
        Unignore
    End Enum

    Class Delete
        Public Time As Date
        Public Page As Page
        Public Action As String
        Public Admin As User
        Public Comment As String
    End Class

    Class EditData
        Public Edit As Edit, Page As Page
        Public Text, Summary, StartTime, EditTime, Token, Section As String
        Public Result As String
        Public CaptchaId, CaptchaWord As String
        Public Minor, Watch, Creating, CannotUndo As Boolean
        Public [Error] As Boolean
        Public NoAutoSummary As Boolean
    End Class

    Class HistoryItem

        Public Edit As Edit
        Public Url As String
        Public Text As String

        Sub New(ByVal Edit As Edit)
            Me.Edit = Edit
        End Sub

        Sub New(ByVal Url As String)
            Me.Url = Url
        End Sub

    End Class

    Class PageMove
        Public Time As Date
        Public User As User
        Public Source As String
        Public Destination As String
        Public Summary As String
    End Class

    Class Protection
        Public Time As Date
        Public Admin As User
        Public Action As String
        Public Page As Page
        Public EditLevel As String
        Public MoveLevel As String
        Public Cascading As Boolean
        Public Expiry As Date
        Public Summary As String
    End Class

    Public Enum ProtectionType As Integer
        Semi
        Full
        Move
    End Enum

    Class SpeedyCriterion
        Public Code As String
        Public DisplayCode As String
        Public Description As String
        Public Template As String
        Public Parameter As String
        Public Message As String
        Public Notify As Boolean
    End Class

    Class Upload
        Public Time As Date
        Public User As User
        Public File As Page
        Public Summary As String
    End Class

    Class Warning
        Public Level As UserLevel
        Public Time As Date
        Public Type As String
        Public User As User
    End Class

    Function ApiLimit() As Integer
        If Administrator Then Return 5000 Else Return 500
    End Function

    Function ArrayContains(Of T)(ByVal Array As T(), ByVal Value As T) As Boolean
        For Each Item As T In Array
            If Item.Equals(Value) Then Return True
        Next Item

        Return False
    End Function

    Sub Callback(ByVal Target As Threading.SendOrPostCallback, _
    Optional ByVal PostData As Object = Nothing)
        'Invoke a method on the main thread
        SyncContext.Post(Target, PostData)
    End Sub

    Sub Callback(ByVal Target As Action)
        'As above, but invoked method does not need to take a parameter
        SyncContext.Post(AddressOf CallbackInvoke, CObj(Target))
    End Sub

    Sub CallbackInvoke(ByVal TargetObject As Object)
        'We're back on the main thread, now invoke the method
        CType(TargetObject, Action)()
    End Sub

    Function CompareUsernames(ByVal a As String, ByVal b As String) As Integer
        Return String.Compare(a, b, StringComparison.OrdinalIgnoreCase)
    End Function

    Function FormatPageHtml(ByVal Page As Page, ByVal Text As String) As String
        'We're only interested in the page here, not the site interface that comes with it.
        'Extract the actual page content and then put some headers back so that it renders properly.
        'No, you can't have your non-monobook skin. So what? It doesn't show the site interface anyway.

        If Text.Contains("<!-- start content -->") AndAlso Text.Contains("<!-- end content -->") Then
            Text = Text.Substring(Text.IndexOf("<!-- start content -->"))
            Text = Text.Substring(0, Text.IndexOf("<!-- end content -->"))

        ElseIf Text.Contains("<!-- content -->") AndAlso Text.Contains("<!-- mw_content -->") Then
            Text = Text.Substring(Text.IndexOf("<!-- content -->"))
            Text = Text.Substring(0, Text.IndexOf("<!-- mw_content -->"))

        ElseIf Text.Contains("</h1>") AndAlso Text.Contains("<div class=""printfooter"">") Then
            Text = Text.Substring(Text.IndexOf("</h1>") + 5)
            Text = Text.Substring(0, Text.IndexOf("<div class=""printfooter"">"))
        End If

        'Modern skin puts scripts in the middle of the page for some reason
        If Text.Contains("<script ") AndAlso Text.Contains("</script>") _
            Then Text = Text.Substring(0, Text.IndexOf("<script ")) & Text.Substring(Text.IndexOf("</script>") + 9)

        Text = "<h1>" & Page.Name & "</h1>" & Text
        Text = MakeHtmlWikiPage(Page.Name, Text)
        Return Text
    End Function

    Function FindString(ByVal Source As String, ByVal [From] As String) As String
        If Not Source.Contains([From]) Then Return Nothing
        Return Source.Substring(Source.IndexOf([From]) + [From].Length)
    End Function

    Function FindString(ByVal Source As String, ByVal [From] As String, ByVal [To] As String) As String
        If Not Source.Contains([From]) Then Return Nothing
        Source = Source.Substring(Source.IndexOf([From]) + [From].Length)
        If Not Source.Contains([To]) Then Return Nothing
        Return Source.Substring(0, Source.IndexOf([To]))
    End Function

    Function FindString(ByVal Source As String, ByVal From1 As String, ByVal From2 As String, _
        ByVal [To] As String) As String

        If Not Source.Contains(From1) Then Return Nothing
        Source = Source.Substring(Source.IndexOf(From1) + From1.Length)
        If Not Source.Contains(From2) Then Return Nothing
        Source = Source.Substring(Source.IndexOf(From2) + From2.Length)
        If Not Source.Contains([To]) Then Return Source
        Return Source.Substring(0, Source.IndexOf([To]))
    End Function

    Function FindString(ByVal Source As String, ByVal From1 As String, ByVal From2 As String, _
        ByVal From3 As String, ByVal [To] As String) As String

        If Not Source.Contains(From1) Then Return Nothing
        Source = Source.Substring(Source.IndexOf(From1) + From1.Length)
        If Not Source.Contains(From2) Then Return Nothing
        Source = Source.Substring(Source.IndexOf(From2) + From2.Length)
        If Not Source.Contains(From3) Then Return Nothing
        Source = Source.Substring(Source.IndexOf(From3) + From3.Length)
        If Not Source.Contains([To]) Then Return Source
        Return Source.Substring(0, Source.IndexOf([To]))
    End Function

    Function GetList(ByVal Value As String) As List(Of String)
        'Converts a comma-separated list to a List(Of String)
        Dim List As New List(Of String)

        For Each Item As String In Value.Replace("\,", Convert.ToChar(1)).Split(","c)
            Item = Item.Trim(" "c, Tab, LF).Replace(Convert.ToChar(1), ",")
            If Not List.Contains(Item) AndAlso Item.Length > 0 Then List.Add(Item)
        Next Item

        Return List
    End Function

    Function GetMonthName(ByVal Number As Integer) As String
        'Yes, I know about MonthName(). But I have to localize.

        Select Case Config.Project
            Case "es.wikipedia"
                Select Case Number
                    Case 1 : Return "enero"
                    Case 2 : Return "febrero"
                    Case 3 : Return "marzo"
                    Case 4 : Return "abril"
                    Case 5 : Return "mayo"
                    Case 6 : Return "junio"
                    Case 7 : Return "julio"
                    Case 8 : Return "agosto"
                    Case 9 : Return "septiembre"
                    Case 10 : Return "octubre"
                    Case 11 : Return "noviembre"
                    Case 12 : Return "diciembre"
                    Case Else : Return ""
                End Select

            Case Else
                Select Case Number
                    Case 1 : Return "January"
                    Case 2 : Return "February"
                    Case 3 : Return "March"
                    Case 4 : Return "April"
                    Case 5 : Return "May"
                    Case 6 : Return "June"
                    Case 7 : Return "July"
                    Case 8 : Return "August"
                    Case 9 : Return "September"
                    Case 10 : Return "October"
                    Case 11 : Return "November"
                    Case 12 : Return "December"
                    Case Else : Return ""
                End Select
        End Select
    End Function

    Function GetPage(ByVal Name As String) As Page
        Return Page.GetPage(Name)
    End Function

    Function GetUser(ByVal Name As String) As User
        Return User.GetUser(Name)
    End Function

    Function HtmlToWikiText(ByVal Text As String) As String
        'Convert edit summary in HTML form to its equivalent wikitext

        If Text.StartsWith("(") AndAlso Text.EndsWith(")") Then Text = Text.Substring(1, Text.Length - 2)

        While Text.Contains("<a href=") AndAlso Text.Contains("</a>")
            Dim LinkTarget As String = HtmlDecode(FindString(Text, "<a href=", "title=""", """"))
            Dim LinkText As String = HtmlDecode(FindString(Text, "<a href=", ">", "</a>"))

            If LinkTarget = LinkText Then
                Text = Text.Substring(0, Text.IndexOf("<a href=")) & "[[" & LinkText & "]]" & _
                    FindString(Text, "</a>")
            Else
                Text = Text.Substring(0, Text.IndexOf("<a href=")) & "[[" & LinkTarget & "|" & LinkText & "]]" & _
                    FindString(Text, "</a>")
            End If
        End While

        Text = StripHTML(Text)

        Return Text
    End Function

    Function IsWikiPage(ByVal Text As String) As Boolean
        'Unfortunately there is no one element common to all skins
        If Text Is Nothing Then Return False
        Return Regex.Match(Text, "<div id=['""](mw[-_])?content['""]>").Success
    End Function

    Function LocalConfigPath() As String
        Dim Path As String = Application.UserAppDataPath
        Path = Path.Substring(0, Path.LastIndexOf("\"))
        Return Path.Substring(0, Path.LastIndexOf("\"))
    End Function

    Sub Log(ByVal Message As String, Optional ByVal Tag As Object = Nothing)
        If MainForm Is Nothing Then LogBuffer.Add(Message) Else MainForm.Log(Message, Tag)
    End Sub

    Function MakeHtmlWikiPage(ByVal Page As String, ByVal Text As String) As String
        Return My.Resources.WikiPageHtml.Replace("$PATH", Config.SitePath).Replace("$PAGE", Page) _
            .Replace("$USER", Config.Username).Replace("$FONTSIZE", CStr(CInt((CInt(Config.DiffFontSize) * 1.2)))) & _
            "<body>" & Text & "</body></html>"
    End Function

    Function Msg(ByVal Name As String, ByVal ParamArray Params() As String) As String
        'Returns a formatted message string localized to the user's language,
        'or in the default language if no localized message is available
        If Config.Messages(Config.Language).ContainsKey(Name) Then
            Return String.Format(Config.Messages(Config.Language)(Name), Params)
        ElseIf Config.Messages(Config.DefaultLanguage).ContainsKey(Name) Then
            Return String.Format(Config.Messages(Config.DefaultLanguage)(Name), Params)
        Else
            Return "[" & Name & "]"
        End If
    End Function

    Function OpenUrlInBrowser(ByVal Url As String) As Boolean
        Try
            Process.Start(Url)
            Return True
        Catch
            Return False
        End Try
    End Function

    Function Ordinal(ByVal N As Integer) As String
        If (N Mod 100) \ 10 = 1 Then Return CStr(N) & "th"

        Select Case N Mod 10
            Case 1 : Return CStr(N) & "st"
            Case 2 : Return CStr(N) & "nd"
            Case 3 : Return CStr(N) & "rd"
            Case Else : Return CStr(N) & "th"
        End Select
    End Function

    Function PageNames(ByVal Pages As List(Of Page)) As List(Of String)
        'Convert a list of pages to a list of their names
        Dim Names As New List(Of String)

        For Each Item As Page In Pages
            If Not Names.Contains(Item.Name) Then Names.Add(Item.Name)
        Next Item

        Return Names
    End Function

    Function ParseUrl(ByVal Url As String) As Dictionary(Of String, String)
        Dim Params As New Dictionary(Of String, String)

        If Url.Contains("wiki/") OrElse Url.Contains("w/index.php/") Then

            If Url.Contains("wiki/") Then Url = Url.Substring(Url.IndexOf("wiki/") + 5) _
                Else Url = Url.Substring(Url.IndexOf("w/index.php/") + 12)

            If Url.Contains("?") Then
                Dim Title As String = Url.Substring(0, Url.IndexOf("?"))

                If Title.Contains("#") Then Title = Title.Substring(0, Title.IndexOf("#"))
                Params.Add("title", UrlDecode(Title.Replace("_", " ")))
                Url = Url.Substring(Url.IndexOf("?") + 1)
            Else
                If Url.Contains("#") Then Url = Url.Substring(0, Url.IndexOf("#"))
                Params.Add("title", UrlDecode(Url.Replace("_", " ")))
                Url = ""
            End If

        ElseIf Url.Contains("w/index.php?") Then
            Url = Url.Substring(Url.IndexOf("w/index.php?") + 12)

        ElseIf Url.Contains("w/wiki.phtml?") Then
            Url = Url.Substring(Url.IndexOf("w/wiki.phtml?") + 13)
        End If

        For Each Item As String In Url.Split("&"c)
            If Item.Contains("=") Then Params.Add(Item.Substring(0, Item.IndexOf("=")), _
                UrlDecode(Item.Substring(Item.IndexOf("=") + 1)))
        Next Item

        Return Params
    End Function

    Function SortWarningsByDate(ByVal X As Warning, ByVal Y As Warning) As Integer
        Return Date.Compare(Y.Time, X.Time)
    End Function

    Function StripHTML(ByVal Text As String) As String
        'Removes anything with < > around it
        If Text Is Nothing Then Return Nothing

        While Text.Contains("<") AndAlso Text.Contains(">")
            Text = Text.Substring(0, Text.IndexOf("<")) & Text.Substring(Text.IndexOf(">") + 1)
        End While

        Return Text
    End Function

    Function Timestamp(ByVal Time As Date) As String
        Return Time.Year & CStr(Time.Month).PadLeft(2, "0"c) & CStr(Time.Day).PadLeft(2, "0"c) & _
            CStr(Time.Hour).PadLeft(2, "0"c) & CStr(Time.Minute).PadLeft(2, "0"c) & CStr(Time.Second).PadLeft(2, "0"c)
    End Function

    Function TrimSummary(ByVal Summary As String) As String
        Summary = Summary.Replace(" (page does not exist)", "")

        While Summary.IndexOf("[[") > -1 AndAlso Summary.IndexOf("[[") < Summary.IndexOf("]]")
            Dim Title As String = Summary.Substring(Summary.IndexOf("[[") + 2)

            If Title.Contains("|") AndAlso (Not Title.Contains("]]") _
                OrElse Title.IndexOf("|") < Title.IndexOf("]]")) _
                Then Title = Title.Substring(Title.IndexOf("|") + 1)

            If Title.Contains("]]") Then Title = Title.Substring(0, Title.IndexOf("]]"))

            Summary = Summary.Substring(0, Summary.IndexOf("[[")) & Title & Summary.Substring(Summary.IndexOf("]]") + 2)
        End While

        Return Summary
    End Function

    Function VersionString(ByVal Version As Version) As String
        Return Version.Major & "." & Version.Minor & "." & Version.Build
    End Function

    Function WikiTimestamp(ByVal Time As Date) As String
        Return CStr(Time.Hour).PadLeft(2, "0"c) & ":" & CStr(Time.Minute).PadLeft(2, "0"c) & ", " & CStr(Time.Day) & _
            " " & GetMonthName(Time.Month) & " " & CStr(Time.Year) & " (UTC)"
    End Function

    Function WikiUrl(ByVal Url As String) As Boolean
        Return (Url.StartsWith(Config.SitePath & "w/index.php?") OrElse Url.StartsWith(Config.SitePath & "wiki/"))
    End Function

End Module