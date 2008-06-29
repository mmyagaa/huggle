Class ConfigForm

    Private ShortcutKeysClone As Dictionary(Of String, Shortcut)

    Private Sub ConfigForm_Load(ByVal s As Object, ByVal e As EventArgs) Handles MyBase.Load
        Tabs.DrawMode = TabDrawMode.OwnerDrawFixed
        Tabs.ItemSize = New Size(1, 1)
        PageBox.SelectedIndex = 0

        AutoWhitelist.Checked = Config.AutoWhitelist
        TrayIcon.Checked = Config.TrayIcon
        ShowQueue.Checked = Config.ShowQueue
        ShowToolTips.Checked = Config.ShowToolTips
        OpenInBrowser.Checked = Config.OpenInBrowser
        ShowNewEdits.Checked = Config.ShowNewEdits
        Preloading.Checked = (Config.Preloads > 0)
        Preloads.Enabled = Preloading.Checked
        Preloads.Text = CStr(Config.Preloads)
        IrcPort.Text = CStr(Config.IrcPort)
        DiffFontSize.Text = Config.DiffFontSize
        ShowAnonymous.Checked = Config.ShowAnonymous
        ShowRegistered.Checked = Config.ShowRegistered
        LogFile.Text = Config.LogFile

        For i As Integer = 0 To Namespaces.Items.Count - 1
            If Config.NamespacesChecked.Contains(CStr(Namespaces.Items(i)).ToLower) _
                Then Namespaces.SetItemChecked(i, True)
        Next i

        ShowNewPages.Checked = Config.ShowNewPages

        If Config.MinorReverts Then Minor.SetItemChecked(0, True)
        If Config.MinorWarnings Then Minor.SetItemChecked(1, True)
        If Config.MinorTags Then Minor.SetItemChecked(2, True)
        If Config.MinorReports Then Minor.SetItemChecked(3, True)
        If Config.MinorNotifications Then Minor.SetItemChecked(4, True)
        If Config.MinorOther Then Minor.SetItemChecked(5, True)

        If Config.WatchReverts Then Watchlist.SetItemChecked(0, True)
        If Config.WatchWarnings Then Watchlist.SetItemChecked(1, True)
        If Config.WatchTags Then Watchlist.SetItemChecked(2, True)
        If Config.WatchReports Then Watchlist.SetItemChecked(3, True)
        If Config.WatchNotifications Then Watchlist.SetItemChecked(4, True)
        If Config.WatchOther Then Watchlist.SetItemChecked(5, True)

        DefaultSummary.Text = Config.DefaultSummary
        UndoSummary.Text = Config.UndoSummary

        ConfirmMultiple.Checked = Config.ConfirmMultiple
        ConfirmSame.Checked = Config.ConfirmSame
        ConfirmSelfRevert.Checked = Config.ConfirmSelfRevert
        AutoAdvance.Checked = Config.AutoAdvance
        UseRollback.Checked = Config.UseRollback

        For Each Item As String In Config.CustomRevertSummaries
            RevertSummaries.Items.Add(Item)
        Next Item

        ClearRevertSummaries.Enabled = (ManualRevertSummaries.Count > 0)

        ReportLinkExamples.Checked = Config.ReportLinkDiffs
        ExtendReports.Enabled = ReportLinkExamples.Checked
        ExtendReports.Checked = Config.ExtendReports
        ReportNone.Checked = (Not Config.AutoReport AndAlso Not Config.PromptForReport)
        ReportPrompt.Checked = Config.PromptForReport
        ReportAuto.Checked = Config.AutoReport

        For Each Item As String In Config.TemplateMessages
            Item = Item.Replace("\;", Chr(1))
            If Item <> "" Then
                Dim NewListItem As New ListViewItem(Item.Substring(0, Item.IndexOf(";")).Replace(Chr(1), ";"))
                NewListItem.SubItems.Add(Item.Substring(Item.IndexOf(";") + 1).Replace(Chr(1), ";"))
                Templates.Items.Add(NewListItem)
            End If
        Next Item

        UseAdminFunctions.Checked = Config.UseAdminFunctions
        PromptForBlock.Checked = Config.PromptForBlock
        BlockReason.Text = Config.BlockReason
        BlockTime.Text = Config.BlockTime
        BlockTimeAnon.Text = Config.BlockTimeAnon

        ColorComment.BackColor = Highlight.CommentC
        ColorExternalLink.BackColor = Highlight.ExternalC
        ColorHtmlTag.BackColor = Highlight.HtmlC
        ColorImage.BackColor = Highlight.ImageHC
        ColorLink.BackColor = Highlight.LinkC
        ColorMagicWord.BackColor = Highlight.MagicWordC
        ColorParam.BackColor = Highlight.ParameterHC
        ColorParamCall.BackColor = Highlight.ParamCallC
        ColorParamName.BackColor = Highlight.ParamNameC
        ColorReference.BackColor = Highlight.ReferenceHC
        ColorTemplate.BackColor = Highlight.TemplateC

        InitialiseShortcutList()
    End Sub

    Private Sub InitialiseShortcutList()
        ShortcutKeysClone = New Dictionary(Of String, Shortcut)(ShortcutKeys)

        ShortcutList.Items.Clear()
        ShortcutList.BeginUpdate()

        For Each Item As KeyValuePair(Of String, Shortcut) In ShortcutKeysClone
            ChangeShortcut.Text = Item.Value.ToString

            Dim NewListViewItem As New ListViewItem
            NewListViewItem.Text = Item.Key
            NewListViewItem.SubItems.Add(Item.Value.ToString)
            ShortcutList.Items.Add(NewListViewItem)
        Next Item

        ShortcutList.Sort()
        ShortcutList.EndUpdate()
    End Sub

    Private Sub ConfigForm_FormClosing(ByVal s As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult = DialogResult.OK Then
            ShortcutKeys = ShortcutKeysClone

            Config.AutoWhitelist = AutoWhitelist.Checked
            Config.TrayIcon = TrayIcon.Checked
            Config.ShowQueue = ShowQueue.Checked
            Config.ShowToolTips = ShowToolTips.Checked
            Config.OpenInBrowser = OpenInBrowser.Checked
            Config.ShowNewEdits = ShowNewEdits.Checked
            If Preloading.Checked Then Config.Preloads = CInt(Preloads.Text) Else Config.Preloads = 0
            Config.IrcPort = CInt(IrcPort.Text)
            Config.DiffFontSize = DiffFontSize.Text
            Config.ShowAnonymous = ShowAnonymous.Checked
            Config.ShowRegistered = ShowRegistered.Checked
            Config.LogFile = LogFile.Text

            Config.NamespacesChecked.Clear()

            For i As Integer = 0 To Namespaces.CheckedItems.Count - 1
                Config.NamespacesChecked.Add(Namespaces.CheckedItems(i).ToString.ToLower)
            Next i

            Config.ShowNewPages = ShowNewPages.Checked

            Config.MinorReverts = Minor.CheckedIndices.Contains(0)
            Config.MinorWarnings = Minor.CheckedIndices.Contains(1)
            Config.MinorTags = Minor.CheckedIndices.Contains(2)
            Config.MinorReports = Minor.CheckedIndices.Contains(3)
            Config.MinorNotifications = Minor.CheckedIndices.Contains(4)
            Config.MinorOther = Minor.CheckedIndices.Contains(5)

            Config.WatchReverts = Watchlist.CheckedIndices.Contains(0)
            Config.WatchWarnings = Watchlist.CheckedIndices.Contains(1)
            Config.WatchTags = Watchlist.CheckedIndices.Contains(2)
            Config.WatchReports = Watchlist.CheckedIndices.Contains(3)
            Config.WatchNotifications = Watchlist.CheckedIndices.Contains(4)
            Config.WatchOther = Watchlist.CheckedIndices.Contains(5)

            Config.DefaultSummary = DefaultSummary.Text
            Config.UndoSummary = UndoSummary.Text

            Config.ConfirmMultiple = ConfirmMultiple.Checked
            Config.ConfirmSame = ConfirmSame.Checked
            Config.ConfirmSelfRevert = ConfirmSelfRevert.Checked
            Config.AutoAdvance = AutoAdvance.Checked
            Config.UseRollback = UseRollback.Checked

            Config.CustomRevertSummaries.Clear()

            For Each Item As String In RevertSummaries.Items
                Config.CustomRevertSummaries.Add(Item)
            Next Item

            Config.ReportLinkDiffs = ReportLinkExamples.Checked
            Config.ExtendReports = ExtendReports.Checked
            Config.AutoReport = ReportAuto.Checked
            Config.PromptForReport = ReportPrompt.Checked

            Config.TemplateMessages.Clear()

            For Each Item As ListViewItem In Templates.Items
                Config.TemplateMessages.Add(Item.Text.Replace(";", "\;") & ";" & _
                    Item.SubItems(1).Text.Replace(";", "\;"))
            Next Item

            Config.UseAdminFunctions = UseAdminFunctions.Checked
            Config.PromptForBlock = PromptForBlock.Checked
            Config.BlockReason = BlockReason.Text
            Config.BlockTime = BlockTime.Text
            Config.BlockTimeAnon = BlockTimeAnon.Text

        Else
            DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub ConfigForm_KeyDown(ByVal s As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If Not ChangeShortcut.Focused AndAlso e.KeyCode = Keys.Escape Then
            DialogResult = DialogResult.Cancel
            Close()
        End If
    End Sub

    Private Sub OK_Click(ByVal s As Object, ByVal e As EventArgs) Handles OK.Click
        Dim NewConfigRequest As New WriteConfigRequest
        NewConfigRequest.Start()

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Click(ByVal s As Object, ByVal e As EventArgs) Handles Cancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub AddTemplate_Click(ByVal s As Object, ByVal e As EventArgs) Handles AddTemplate.Click
        Dim NewAddTemplateForm As New AddTemplateForm

        If NewAddTemplateForm.ShowDialog = DialogResult.OK Then
            Dim NewListItem As New ListViewItem(NewAddTemplateForm.DisplayText)
            NewListItem.SubItems.Add(NewAddTemplateForm.Template)
            Templates.Items.Add(NewListItem)
        End If
    End Sub

    Private Sub RemoveTemplate_Click(ByVal s As Object, ByVal e As EventArgs) Handles RemoveTemplate.Click
        If Templates.SelectedItems.Count > 0 Then Templates.Items.Remove(Templates.SelectedItems(0))
    End Sub

    Private Sub Templates_SelectedIndexChanged(ByVal s As Object, ByVal e As EventArgs) _
        Handles Templates.SelectedIndexChanged

        RemoveTemplate.Enabled = (Templates.SelectedItems.Count > 0)
    End Sub

    Private Sub RevertSummaries_SelectedIndexChanged(ByVal s As Object, ByVal e As EventArgs) _
        Handles RevertSummaries.SelectedIndexChanged

        RemoveSummary.Enabled = (RevertSummaries.SelectedIndex > -1)
    End Sub

    Private Sub AddSummary_Click(ByVal s As Object, ByVal e As EventArgs) Handles AddSummary.Click
        Dim Item As String = InputBox("Enter summary", "Add summary")
        If Item <> "" Then RevertSummaries.Items.Add(Item)
    End Sub

    Private Sub RemoveSummary_Click(ByVal s As Object, ByVal e As EventArgs) Handles RemoveSummary.Click
        If RevertSummaries.SelectedIndex > -1 Then RevertSummaries.Items.RemoveAt(RevertSummaries.SelectedIndex)
    End Sub

    Private Sub ShowAnonymous_CheckedChanged(ByVal s As Object, ByVal e As EventArgs) _
        Handles ShowAnonymous.CheckedChanged

        If Not ShowAnonymous.Checked Then ShowRegistered.Checked = True
    End Sub

    Private Sub ShowRegistered_CheckedChanged(ByVal s As Object, ByVal e As EventArgs) _
        Handles ShowRegistered.CheckedChanged

        If Not ShowRegistered.Checked Then ShowAnonymous.Checked = True
    End Sub

    Private Sub Preloading_CheckedChanged(ByVal s As Object, ByVal e As EventArgs) Handles Preloading.CheckedChanged
        Preloads.Enabled = (Preloading.Checked)
    End Sub

    Private Sub ReportLinkExamples_CheckedChanged(ByVal s As Object, ByVal e As EventArgs) _
        Handles ReportLinkExamples.CheckedChanged

        ExtendReports.Enabled = ReportLinkExamples.Checked
    End Sub

    Private Sub LogFileBrowse_Click(ByVal s As Object, ByVal e As EventArgs) Handles LogFileBrowse.Click
        Dim Dialog As New SaveFileDialog

        Dialog.Title = "Log file location"
        Dialog.FileName = "huggle.log"
        Dialog.Filter = "Text file|*.txt"

        If Dialog.ShowDialog = DialogResult.OK Then LogFile.Text = Dialog.FileName
    End Sub

    Private Sub NewShortcut_GotFocus(ByVal s As Object, ByVal e As EventArgs) Handles ChangeShortcut.GotFocus
        ChangeShortcut.Clear()
    End Sub

    Private Sub Shortcuts_MouseUp(ByVal s As Object, ByVal e As MouseEventArgs) Handles ShortcutList.MouseUp
        ChangeShortcut.Focus()
    End Sub

    Private Sub Shortcuts_SelectedIndexChanged(ByVal s As Object, ByVal e As EventArgs) _
        Handles ShortcutList.SelectedIndexChanged

        If ShortcutList.SelectedItems.Count > 0 Then
            ChangeShortcutLabel.Visible = True
            ChangeShortcutLabel.Text = "Change shortcut for " & ShortcutList.SelectedItems(0).Text & ":"
            NoShortcut.Visible = True
            ChangeShortcut.Visible = True
            ChangeShortcut.Text = ShortcutList.SelectedItems(0).SubItems(1).Text
            ChangeShortcut.Focus()
        End If
    End Sub

    Private Sub NewShortcut_KeyDown(ByVal s As Object, ByVal e As KeyEventArgs) Handles ChangeShortcut.KeyDown
        e.SuppressKeyPress = True

        If e.KeyCode <> Keys.ControlKey AndAlso e.KeyCode <> Keys.ShiftKey AndAlso e.KeyCode <> Keys.Alt _
            AndAlso ShortcutList.SelectedItems.Count > 0 Then

            Dim NewShortcut As New Shortcut(e.KeyCode, e.Control, e.Alt, e.Shift)

            'Detect conflicts
            For Each Item As KeyValuePair(Of String, Shortcut) In ShortcutKeys
                If Item.Key <> ShortcutList.SelectedItems(0).Text AndAlso Item.Value = NewShortcut Then
                    MsgBox("Shortcut '" & NewShortcut.ToString & "' conflicts with the existing shortcut for '" & _
                        Item.Key & "'.", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            Next Item

            ChangeShortcut.Text = NewShortcut.ToString

            ShortcutKeysClone(ShortcutList.SelectedItems(0).Text) = NewShortcut
            ShortcutList.SelectedItems(0).SubItems(1).Text = NewShortcut.ToString
        End If
    End Sub

    Private Sub NoShortcut_Click(ByVal s As Object, ByVal e As EventArgs) Handles NoShortcut.Click
        If ShortcutList.SelectedItems.Count > 0 Then
            ShortcutKeysClone(ShortcutList.SelectedItems(0).Text) = New Shortcut(Keys.None)
            ShortcutList.SelectedItems(0).SubItems(1).Text = "None"
            ChangeShortcut.Clear()
            ChangeShortcut.Focus()
        End If
    End Sub

    Private Sub Defaults_Click(ByVal s As Object, ByVal e As EventArgs) Handles Defaults.Click
        If MsgBox("Restore defaults?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            InitialiseShortcuts()
            InitialiseShortcutList()
            ChangeShortcut.Clear()
        End If
    End Sub

    Private Sub ClearRevertSummaries_Click(ByVal s As Object, ByVal e As EventArgs) Handles ClearRevertSummaries.Click
        ManualRevertSummaries.Clear()
    End Sub

    Private Sub PageBox_SelectedIndexChanged(ByVal s As Object, ByVal e As EventArgs) _
        Handles PageBox.SelectedIndexChanged

        Tabs.SelectedIndex = PageBox.SelectedIndex
    End Sub

    Private Sub SetColor(ByVal sender As Object, ByVal e As EventArgs) Handles ColorComment.Click, _
        ColorExternalLink.Click, ColorHtmlTag.Click, ColorImage.Click, ColorLink.Click, ColorMagicWord.Click, _
        ColorParam.Click, ColorParamCall.Click, ColorParamName.Click, ColorReference.Click, ColorTemplate.Click

        Dim Control As Control = CType(sender, Control)
        Dim NewColorDialog As New ColorDialog
        NewColorDialog.AnyColor = True
        NewColorDialog.FullOpen = True
        NewColorDialog.Color = Control.BackColor
        If NewColorDialog.ShowDialog = DialogResult.OK Then Control.BackColor = NewColorDialog.Color
    End Sub

End Class