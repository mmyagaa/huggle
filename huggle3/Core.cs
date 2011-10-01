﻿//This is a source code or part of Huggle project
//
//This file contains code for
//last modified by Petrb

//Copyright (C) 2011 Huggle team
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace huggle3
{
    public static class Languages
    {
        public static string Get(string id)
        {
            try
            {
                if (Config.Messages.ContainsKey(Config.Language) != true)
                {
                    return "<invalid>";
                }
                if (Config.Messages[Config.Language].ContainsKey(id) == false)
                {
                    if (Config.Messages[Config.DefaultLanguage].ContainsKey(id))
                    {
                        if (Config.Messages[Config.DefaultLanguage][id] == null)
                        {
                            return "<invalid>";
                        }
                        return Config.Messages[Config.DefaultLanguage][id];
                    }
                }
                else
                {
                    // got it
                    if (Config.Messages[Config.Language][id] == null)
                    {
                        return "";
                    }
                    return Config.Messages[Config.Language][id];
                }
            }
            catch (Exception A)
            {
                Core.ExceptionHandler( A );
            }

            return "<invalid>";
        }
    }
    public static class Core
    {
        private static string _history = "";

        private static Exception core_er;

        public static Dictionary<page, string> CustomReverts = new Dictionary<page,string>();
        public static queue Current_Queue;
        public static bool Interrupted = false;
        public static string EditToken;
        public static bool HidingEdit = false;
        public static System.DateTime LastRCTime = new System.DateTime();
        public static edit NullEdit;
        public static string Patrol_Token;
        public static System.Threading.Thread MainThread;
        public static string[] months;


        public class Block
        {
            public System.DateTime Block_Date;
            public string Block_Comment;
            public string Block_Duration;
            public string Block_Action;
            public user Block_Sysop;
            public user Block_User;
        }

        public class CacheData
        {
            public edit Edit;
            public string Text;
        }

        public class HistoryItem
        {
            public string text;
            public string url;
            public edit Edit;
            public HistoryItem(string _url)
            {
                this.url = _url;
            }

            public HistoryItem(edit _edit)
            {
                this.Edit = _edit;
            }
        }

        public class PageMove
        {
            public System.DateTime Date;
        }

        public class Protection
        {
            public bool Cascading;
            public bool Pending;
            public string MoveLevel;
            public string CreateLevel;
            public string Action;
            public string Summary;
            public string EditLevel;
            public System.DateTime Date;
            public user Sysop;
        }


        public class Command
        {
            public edit Edit;
            public string Description;
            public user User;
            public page Page;
        }

        enum CommandType
        {
            Revert,
            Report,
            Warning,
            Edit,
            Ignore,
        }

        public class EditData
        {
            public edit Edit;
            public page Page;
            public string CaptchaWord; // deprecated
            public string CaptchaId;
            public string Text;
            public string Summary;
            public string Section;
            public string Token;
            public string EditTime;
            public bool Error;
            public bool Creating;
            public bool Minor;
            public bool CannotUndo;
            public bool Watch;
            public bool AutoSummary;
        }

        public class Upload
        {
            public user User;
            public page File;
        }

        public class Warning
        {
            public System.DateTime Date;
            public user User;
            
        }

        public static string LocalPath()
        {
            return Application.LocalUserAppDataPath + "huggle3" + Path.DirectorySeparatorChar;
        }

        public static void ShutdownSystem()
        {
            Application.Exit();
        }

        public static bool History(string text)
        {
            if (_history.Length - Config.HistoryTrim > Config.HistoryLenght)
            {
                _history = _history.Substring(Config.HistoryTrim);
                _history = "{trimmed} " + _history;
            }
            _history = _history + " -> " + text;
            return false;
        }

        public static void callback(System.Threading.SendOrPostCallback Target, Object PostdData)
        {
            Core.History("callback()");
        }

        public static string FindString(string Source, string from1, string from2, string To)
        {
            //this function look up a string
            Core.History("FindString(string, string, string, string)");
            if (Source == null)
            {    return null; }
            
            if (Source.Contains(from1) != true)
            {
                return null;
            }

            Source = Source.Substring(Source.IndexOf(from1) + from1.Length);
            if (Source.Contains(from2) != true)
            {
                return null;
            }
            Source = Source.Substring(Source.IndexOf(from2) + from2.Length);
            if (Source.Contains(To))
            {
                return Source.Substring(0, Source.IndexOf(To));
            }
            return "";
        }

        public static string FindString(string Source, string from1, string from2, string from3, string To)
        {
            // same one
            Core.History("FindString(string, string, string, string, string)");
            string temp_v = Source;
            Source = null;
            if (tmp_v.Contains(from1) != true)
            {
                return null;
            }
            temp_v = temp_v.Substring(temp_v.IndexOf(from1) + from1.Length);
            if (temp_v.Contains(from2) != true)
            {
                return null;
            }
            temp_v = temp_v.Substring(temp_v.IndexOf(from2) + from2.Length);
            if (temp_v.Contains(To))
            {
                return temp_v.Substring(0, Source.IndexOf(To));
            }
            return Source;
        }

        public static string FormatHTML(page Page, string Text)
        {
            History("Core.FormatHTML (" + Page + " )");
            try
            {
                string return_value = "";
                if (Text.Contains("<!-- start content -->") && Text.Contains("<!-- end content -->") && Text != "")
                {
                    return_value = Text.Substring(Text.IndexOf("<!-- start content -->"));
                    return_value = return_value.Substring(0, return_value.IndexOf("<!-- end content -->"));
                }
                else if (Text.Contains("<!-- content -->") && Text.Contains("<!-- mw_content -->"))
                {
                    return_value = Text.Substring(Text.IndexOf("<!-- content -->"));
                    return_value = return_value.Substring(0, return_value.IndexOf("<!-- mw_content -->"));
                }
                else if (Text.Contains("</h1>") && Text.Contains("<div class=\"printfooter\">"))
                {
                    return_value = Text.Substring(Text.IndexOf("</h1>"));
                    return_value = return_value.Substring(0, return_value.IndexOf("<div class=\"printfooter\">"));
                }

                if (Text.Contains("<script>") && Text.Contains("</script>"))
                {

                }
                return_value = "<h1>" + Page.Name + "</h1>" + return_value;

                return return_value;
            }
            catch (Exception B)
            { Core.ExceptionHandler(B); }
            return "";
        }

        public static bool IsMW(string Content)
        {
            if (Content == null)
            {
                return false;
            }
            return System.Text.RegularExpressions.Regex.Match(Content, "<body class=.mediawiki").Success;
        }

        public static string FindString(string Source, string From)
        {
            Core.History("Core.FindString( string, string)");
            if (Source.Contains(From))
            {
                return Source.Substring(Source.IndexOf(From) + From.Length);
            }

            return "";
        }

        public static bool InitConfig()
        {   // this function initialise config
            // reset values
            // those values will be default in case that not present in configs
            // do not change unless you want to change default presets
            Core.History("Core.InitConfig()");
            Config.AIVLocation = "";
            Config.Approval = false;
            Config.AutoAdvance = false;
            Config.AutoReport = false;
            Config.AutoWarn = false;
            Config.AutoWhitelist = true;
            Config.Block = false;
            Config.BlockMessage = "";
            Config.BlockMessageDefault = false;
            Config.BlockReason = "Vandalism";
            Config.BlockSummary = "Blocked";
            Config.BlockTime = "0";
            Config.BlockTimeAnon = "0";
            Config.ConfigChanged = false;
            Config.ConfigSummary = "";
            Config.ConfirmIgnored = false;
            Config.ConfirmMultiple = false;
            Config.ConfirmPage = false;
            Config.ConfirmRange = false;
            Config.ConfirmSame = false;
            Config.ConfirmSelfRevert = true;
            Config.ConfirmWarned = false;
            Config.Csd_Log_Page = "Special:MyPage/HgLogs";
            Config.DefaultSummary = "";
            Config.Delete = false;
            Config.Email = false;
            Config.Enabled = false;
            Config.EnabledForAll = false;
            Config.ExtendReports = false;
            Config.FeedbackLocation = "WP:Huggle/Feedback";
            Config.MonthHeadings = true;
            Config.Password = "";
            Config.Patrol = true;
            Config.PatrolSpeedy = true;
            Config.Prod = false;
            Config.ProdLogs = true;
            Config.ProdMessage = "";
            Config.ProdMessageSummary = "";
            Config.Project = "";
            Config.ProtectionReason = "";
            Config.ProtectionRequestPage = "";
            Config.ProtectionRequestReason = "";
            Config.ProtectionRequests = false;
            Config.ProtectionRequestSummary = "";
            Config.ProtectionTime = "";
            Config.ProxyEnabled = false;
            Config.ProxyPort = "";
            Config.QuickSight = false;
            Config.RememberMe = false;
            Config.RememberPassword = false;
            Config.DefaultLanguage = "en";
            Core.months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return true;
        }

        public static string history
        {
            get { return _history; }
        }

        public static void LoadLanguages()
        {
            Config.Messages.Clear();
            Core.History("Core.LoadLanguages()");
            Load_Language("de", huggle3.Properties.Resources.de);
            Load_Language("en", huggle3.Properties.Resources.en);
            Load_Language("es", huggle3.Properties.Resources.es);
            Load_Language("fa", huggle3.Properties.Resources.fa);
            Load_Language("fr", huggle3.Properties.Resources.fr);
            Load_Language("hi", huggle3.Properties.Resources.hi);
            Load_Language("it", huggle3.Properties.Resources.it);
            Load_Language("ja", huggle3.Properties.Resources.ja);
            Load_Language("ka", huggle3.Properties.Resources.ka);
            Load_Language("kn", huggle3.Properties.Resources.kn);
            Load_Language("ml", huggle3.Properties.Resources.ml);
            Load_Language("mr", huggle3.Properties.Resources.mr);
            Load_Language("bg", huggle3.Properties.Resources.bg);
            Load_Language("nl", huggle3.Properties.Resources.nl);
            Load_Language("no", huggle3.Properties.Resources.no);
            Load_Language("oc", huggle3.Properties.Resources.oc);
            Load_Language("or", huggle3.Properties.Resources.or);
            Load_Language("pt", huggle3.Properties.Resources.pt);
            Load_Language("ru", huggle3.Properties.Resources.ru);
            Load_Language("sv", huggle3.Properties.Resources.sv);
            Load_Language("zh", huggle3.Properties.Resources.zh);
            Load_Language("ar", huggle3.Properties.Resources.ar);
        }

        public static bool StopAll()
        {
            // stop everything in system
            return false;
        }

        public static void Load_Language(string language, string data)
        {
            if (Config.Languages.Contains(language) == false)
            {
                if (Config.Messages.ContainsKey(language))
                {
                    Config.Messages.Remove(language);
                }
                Config.Messages.Add(language , new Dictionary<string, string>());
                foreach ( string message in data.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries) )
                {
                    if ( message.Contains(":") )
                    {
                        string message_value = message.Substring(message.IndexOf(":") + 1).Trim(' ').Replace("\n", "").Replace(Convert.ToChar(13).ToString(), "").Replace(Convert.ToChar(10).ToString(), "");
                        string message_name = message.Substring(0, message.IndexOf(":")).Trim(' ');
                        Config.Messages[language].Add(message_name, message_value);
                    }
                }
                Config.Languages.Add(language);
            }
        }

        public static void Suspend()
        {
            // Suspend thread
            Core.Interrupted = true;
            while (Core.Interrupted)
            {
                System.Threading.Thread.Sleep(2000);
            }
        }

        public static void Initialise()
        {   
            //this function is called when the application start
            Core.History("Core.Initialise()");
            Config.DefaultLanguage = "en";
            MainThread = System.Threading.Thread.CurrentThread;
            InitConfig();
            Config.Language = Config.DefaultLanguage;
            LoadLanguages();
        }

        public static bool ExceptionHandler(Exception error_handle, bool panic = false)
        {
            System.Threading.Thread Recovery = new System.Threading.Thread(CreateEx);
            
            core_er = error_handle;
            Recovery.Name = "recovery thread";
            Recovery.Start();
            if (panic == true)
            {
                StopAll();
            }
            if (MainThread == System.Threading.Thread.CurrentThread)
            {
                Core.Suspend();
            }
            
            return true;
        }

        public static void CreateEx()
        {
            huggle3.Forms.ExceptionForm fx = new huggle3.Forms.ExceptionForm();
            fx.error = core_er;
            Application.Run(fx);
        }

        public static string MakePath(string[] Items)
        {

            return "";
        }

        public static page GetPage(string PageName)
        {
            // get a new page
            Core.History("GetPage()");
            try
            {
                page Page = new page();
                Page.Name = PageName;
                return Page;
            }
            catch (Exception weird)
            {
                // weird, probably out of memory or something like that
                Core.ExceptionHandler(weird);
            }
            return null;
        }
    }
    public static class Core_Scripting
    {
        public class plugin
        {
            public int ID;
            
        }

        public static string Main = "main.tcl";
        public static bool Enabled = true;
    }
    public static class Core_IO
    {
        public static class GET
        {
            public static Dictionary<string, string> dictionary(string data)
            {
                Dictionary<string, string> return_value = new Dictionary<string,string>();
                string current_value;

                foreach ( string Item in GET.list( data ) )
                {
                    current_value = Item;
                    current_value = current_value.Trim ( ' ', '\t', '\n' ).Replace( "\\;", Convert.ToChar(2 ).ToString());
                    
                    if ( current_value.Contains ( ";" ) )
                    {
                        string KEY = current_value.Split ( ';' )[0].Replace ( Convert.ToChar ( 2 ), ';' );
                        string VAL = current_value.Split ( ';' )[1].Replace ( Convert.ToChar ( 2 ), ';' );
                        if ( ! return_value.ContainsKey( KEY ) )
                        {
                            return_value.Add ( KEY, VAL );
                        }
                    }
                }

                return return_value;
            }
            public static bool Months(string data)
            {
                return true;
            }
            public static List<string> list(string data)
            {
                //parse
                List<string> DATA = new List<string>();
                string current_value;
                foreach (string x in data.Replace(Convert.ToChar(2).ToString(), "").Split(','))
                {
                    current_value = x;
                    current_value = current_value.Trim(' ', '\t', '\n').Replace(Convert.ToChar(1).ToString(), ",");
                    if (DATA.Contains(current_value) != true && current_value != "")
                    {
                        DATA.Add(current_value);
                    }
                }
                return DATA;
            }
            public static List<List<string>> RecordList(int fields, string text)
            {
                List<List<string>> return_value = new List<List<string>>();

                return return_value;
            }
        }
        public static Dictionary<string, string> ProcessConfigFile (string name)
        {
            Core.History("CoreIO.ProcessConfigFile()");
            List<string> Items = new List<string>(name.Replace("\t","    ").Split('\n'));
            Dictionary<string, string> value = new Dictionary<string, string>();

            int Indent = 0;
            int i = 0;
            
            while ( i < Items.Count )
            {
            if ( ( i > 0 ) && ( Items[i].StartsWith(" ") ) && ( Items[i].Replace(" ", "") != "" ) )
            {
                if ( Indent == 0 )
                    {
                                while ( Items[i][Indent] == ' ' )
                                {
                                    Indent += 1;
                                }
                    }

                Items[i] = Items[i].Substring(Indent);
                Items[i - 1] = Items[i - 1] + Convert.ToChar(2) + Items[i].TrimEnd(' ');
                Items.RemoveAt(i);
            }
            else if ( Items[i].StartsWith("#") || Items[i].StartsWith("<") ||  ( ! Items[i].Contains(":") ) )
                {
                    Items.RemoveAt(i);
                }
            else
                {
                char LF = (char)10;
                Indent = 0;
                Items[i] = Items[i].Replace("\n", LF.ToString()).Trim(' ');
                i++;
                }
            }
        
            foreach ( string Item in Items )
            {
                string Name = Item.Split(':')[0].Trim(Convert.ToChar( 2 ));
                string Value = Item.Substring(Item.IndexOf(":") + 1).Trim(Convert.ToChar( 2 )).Trim(Convert.ToChar( 13 ));

                if ( ! value.ContainsKey(Name) )
                {
                    value.Add(Name, Value);
                }
            }
            return value;
        }
        public static bool SetLocalConfigOption(string key, string value)
        {
            switch (key)
            { 
                case "language":
                     Config.Language = value;
                     break;
                case "log-file":
                     Config.LogFile = value;
                     break;
                case "password":
                     Config.RememberPassword = true;
                     Config.Password = value;
                     break;
                case "projects":
                     Config.Projects = GET.dictionary(value);
                     if ( Config.Projects.ContainsKey("test2") == false )
                     {
                        Config.Projects.Add("test2", Config.TestWp);
                     }
                    break;
                 case "project":
                     Config.Project = value;
                     break;
                 case "proxy-enabled":
                     Config.ProxyEnabled = Boolean.Parse( value );
                     break;
                 case "proxy-port":
                     Config.ProxyPort = value;
                     break;
                 case "proxy-server":
                     Config.ProxyServer = value;
                     break;
                 case "proxy-userdomain":
                     Config.ProxyUserDomain = value;
                     break;
                 case "proxy-username":
                     Config.ProxyUsername = value;
                     break;
                 case "queue-right-align":
                     Config.RightAlignQueue = Boolean.Parse(value);
                     break;
                 case "revert-summaries":
                     Config.RevertSummaries = GET.list(value);
                     break;
                //case "shortcuts" : SetShortcuts(Value)
                 case "show-new-messages" :
                     Config.ShowNewMessages = bool.Parse(value);
                     break;
                 case "show-two-queues":
                     Config.ShowTwoQueues = Boolean.Parse(value);
                     break;
                case "TestWp":
                     Config.TestWp = value;
                     break;
                 case "username":
                     Config.Username = value;
                     break;
                 case "whitelist-timestamps":
                     Config.WhitelistTimestamps = GET.dictionary(value);
                     break;
                 case "window-height":
                     Config.WindowSize.Height = int.Parse (value);
                     break;
                 //case "window-left":Config.WindowPosition.X = CInt(Value)
                 case "window-maximize":
                     Config.WindowMaximize = Boolean.Parse(value);
                     break;
                 //case "window-top" : Config.WindowPosition.Y = CInt(Value)
            }
                    return true;
        }
        public static bool SetSharedConfigKey(string key, string value)
        {
            switch (key)
            {
                case "admin":
                    Config.UseAdminFunctions = Boolean.Parse(value);
                    break;
                case "aiv-extend-reports":
                    Config.ExtendReports = Boolean.Parse(value);
                    break;
                case "blocktime":
                    Config.BlockTime = value;
                    break;
                case "prodlogs":
                    break;
                case "blocktime-anon":
                    Config.BlockTimeAnon = value;
                    break;
                case "block-message-default":
                    Config.BlockMessageDefault = Boolean.Parse(value);
                    break;
                case "block-message":
                    Config.BlockMessage = value;
                    break;
                case "block-prompt":
                    Config.PromptForBlock = Boolean.Parse(value);
                    break;
                case "block-summary":
                    Config.BlockSummary = value;
                    break;
                case "confirm-ignored":
                    Config.ConfirmIgnored = Boolean.Parse(value);
                    break;
                case "confirm-multiple":
                    Config.ConfirmMultiple = Boolean.Parse(value);
                    break;
                case "confirm-same":
                    Config.ConfirmSame = Boolean.Parse(value);
                    break;
                case "confirm-page":
                    Config.ConfirmPage = Boolean.Parse(value);
                    break;
                case "confirm-range":
                    Config.ConfirmRange = Boolean.Parse(value);
                    break;
                case "confirm-self-revert":
                    Config.ConfirmSelfRevert = false;
                    break;
                case "confirm-warned":
                    Config.ConfirmWarned = Boolean.Parse(value);
                    break;
                case "default-summary":
                    Config.DefaultSummary = value;
                    break;
                case "diff-font-size":
                    Config.DiffFontSize = value;
                    break;
                case "irc":
                    Config.UseIrc = Boolean.Parse(value);
                    break;
                case "rollback":
                    Config.RequireRollback = Boolean.Parse(value);
                    break;
                case "minor":
                    //Config.Minor = Boolean.Parse(value);
                    break;
                case "open-in-browser":
                    Config.OpenInBrowser = Boolean.Parse(value);
                    break;
                case "patrol-speedy":
                    Config.PatrolSpeedy = Boolean.Parse(value);
                    break;
                case "preload":
                    Config.Preloads = int.Parse(value);
                    break;
                case "prod":
                    Config.Prod = Boolean.Parse(value);
                    break;
                case "prod-log":
                    Config.ProdLogs = Boolean.Parse(value);
                    break;
                case "prod-message":
                    Config.ProdMessage = value;
                    break;
                case "prod-page":
                    Config.ProdLogs_Name = value;
                    break;
                case "prod-message-summary":
                    Config.ProdMessageSummary = value;
                    break;
                case "prod-message-title":
                    Config.ProdMessageTitle = value;
                    break;
                case "prod-summary":
                    Config.ProdSummary = value;
                    break;
                case "protection-reason":
                    Config.ProtectionReason = value;
                    break;
                case "show-queue":
                    Config.ShowQueue = Boolean.Parse(value);
                    break;
                case "show-tool-tips":
                    Config.ShowToolTips = Boolean.Parse(value);
                    break;
                case "speedy-message-title":
                    Config.SpeedyMessageTitle = value;
                    break;
                case "speedy-summary":
                    Config.SpeedySummary = value;
                    break;
                case "tray-icon":
                    Config.TrayIcon = Boolean.Parse(value);
                    break;
                case "undo-summary":
                    Config.UndoSummary = value;
                    break;
                case "update-whitelist":
                    Config.UpdateWhitelist = Boolean.Parse(value);
                    break;
                case "vandal-report-reason":
                    Config.VandalReportReason = value;
                    break;
                case "welcome":
                    Config.Welcome = value;
                    break;
                case "welcome-anon":
                    Config.WelcomeAnon = value;
                    break;
                case "warn-summary":
                    Config.WarnSummary = value;
                    break;
                case "protection-requests":
                    Config.ProtectionRequests = Boolean.Parse(value);
                    break;
                case "irc-port":
                    Config.IrcPort = int.Parse(value);
                    break;
                case "enable":
                    Config.Enabled = Boolean.Parse(value);
                    break;

            }
            return true;
        }
        public static bool LoadLocalConfig()
        {
            Core.History("LoadLocalConfig()");
            if (!System.IO.File.Exists(Core.LocalPath() + Config.LocalConfigLocation))
            {
                try
                {
                    if ( ! Directory.Exists(Core.LocalPath() ))
                    {
                        Directory.CreateDirectory(Core.LocalPath());
                    }
        
                
                    File.WriteAllText((Core.LocalPath() + Config.LocalConfigLocation), huggle3.Properties.Resources.DefaultLocalConfig);
                } catch ( DirectoryNotFoundException A )
                    {
                        Core.ExceptionHandler(A);
                        return true;
                    }
            }

            if  ( System.IO.File.Exists(Core.LocalPath() + Config.LocalConfigLocation) )
            {
                foreach (KeyValuePair<string,string> Item in ProcessConfigFile(File.ReadAllText(Core.LocalPath() + Config.LocalConfigLocation)))
                {
                    SetLocalConfigOption(Item.Key, Item.Value);
                }
            }
            if (Config.Projects.Count == 0)
            { 
                foreach ( KeyValuePair<string,string> Item in ProcessConfigFile(huggle3.Properties.Resources.DefaultLocalConfig) )
                {
                    SetLocalConfigOption(Item.Key, Item.Value);
                }
            }
            return true;
        }
        public static bool LoadGlobalConfig()
        {
            Core.History("LoadGlobalConfig()");
            return true;
        }
    }

}