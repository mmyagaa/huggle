
// This file has been generated by the GUI designer. Do not modify.
namespace huggle3.Forms
{
    public partial class Login
    {
        private global::Gtk.VBox vbox1;
        private global::Gtk.Image image1;
        private global::Gtk.Table table1;
        private global::Gtk.ComboBox combobox1;
        private global::Gtk.ComboBox combobox2;
        private global::Gtk.Entry entry1;
        private global::Gtk.Entry entry2;
        private global::Gtk.Label label1;
        private global::Gtk.Label label2;
        private global::Gtk.Label label3;
        private global::Gtk.Label label4;
        private global::Gtk.CheckButton checkbutton1;
        private global::Gtk.ProgressBar progressbar1;
        private global::Gtk.Label label5;
        private global::Gtk.Button button1;
        private global::Gtk.HBox hbox1;
        private global::Gtk.VBox vbox2;
        private global::Gtk.Label label6;
        private global::Gtk.Label label7;
        private global::Gtk.Label label8;
        private global::Gtk.Button button2;
        
        protected virtual void Build ()
        {
            global::Stetic.Gui.Initialize (this);
            // Widget huggle3.Forms.Login
            this.Name = "huggle3.Forms.Login";
            this.Title = global::Mono.Unix.Catalog.GetString ("Huggle");
            this.Icon = global::Gdk.Pixbuf.LoadFromResource ("huggle3.Pictures.huggle.ico");
            this.WindowPosition = ((global::Gtk.WindowPosition)(1));
            this.Resizable = false;
            // Container child huggle3.Forms.Login.Gtk.Container+ContainerChild
            this.vbox1 = new global::Gtk.VBox ();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.image1 = new global::Gtk.Image ();
            this.image1.Name = "image1";
            this.image1.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("huggle3.Pictures.huggle3_newlogo.png");
            this.vbox1.Add (this.image1);
            global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.image1]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.table1 = new global::Gtk.Table (((uint)(4)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.combobox1 = global::Gtk.ComboBox.NewText ();
            this.combobox1.WidthRequest = 245;
            this.combobox1.Name = "combobox1";
            this.table1.Add (this.combobox1);
            global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this.combobox1]));
            w2.TopAttach = ((uint)(2));
            w2.BottomAttach = ((uint)(3));
            w2.LeftAttach = ((uint)(1));
            w2.RightAttach = ((uint)(2));
            w2.XOptions = ((global::Gtk.AttachOptions)(4));
            w2.YOptions = ((global::Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.combobox2 = global::Gtk.ComboBox.NewText ();
            this.combobox2.Name = "combobox2";
            this.table1.Add (this.combobox2);
            global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.combobox2]));
            w3.TopAttach = ((uint)(3));
            w3.BottomAttach = ((uint)(4));
            w3.LeftAttach = ((uint)(1));
            w3.RightAttach = ((uint)(2));
            w3.XOptions = ((global::Gtk.AttachOptions)(4));
            w3.YOptions = ((global::Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.entry1 = new global::Gtk.Entry ();
            this.entry1.CanFocus = true;
            this.entry1.Name = "entry1";
            this.entry1.IsEditable = true;
            this.entry1.InvisibleChar = '●';
            this.table1.Add (this.entry1);
            global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1 [this.entry1]));
            w4.LeftAttach = ((uint)(1));
            w4.RightAttach = ((uint)(2));
            w4.XOptions = ((global::Gtk.AttachOptions)(4));
            w4.YOptions = ((global::Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.entry2 = new global::Gtk.Entry ();
            this.entry2.CanFocus = true;
            this.entry2.Name = "entry2";
            this.entry2.IsEditable = true;
            this.entry2.InvisibleChar = '●';
            this.table1.Add (this.entry2);
            global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this.entry2]));
            w5.TopAttach = ((uint)(1));
            w5.BottomAttach = ((uint)(2));
            w5.LeftAttach = ((uint)(1));
            w5.RightAttach = ((uint)(2));
            w5.XOptions = ((global::Gtk.AttachOptions)(4));
            w5.YOptions = ((global::Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label1 = new global::Gtk.Label ();
            this.label1.Name = "label1";
            this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("[[login-username]]");
            this.table1.Add (this.label1);
            global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
            w6.XOptions = ((global::Gtk.AttachOptions)(4));
            w6.YOptions = ((global::Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label2 = new global::Gtk.Label ();
            this.label2.Name = "label2";
            this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("[[login-password]]");
            this.table1.Add (this.label2);
            global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.label2]));
            w7.TopAttach = ((uint)(1));
            w7.BottomAttach = ((uint)(2));
            w7.XOptions = ((global::Gtk.AttachOptions)(4));
            w7.YOptions = ((global::Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label3 = new global::Gtk.Label ();
            this.label3.Name = "label3";
            this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("[[login-project]]");
            this.table1.Add (this.label3);
            global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.label3]));
            w8.TopAttach = ((uint)(2));
            w8.BottomAttach = ((uint)(3));
            w8.XOptions = ((global::Gtk.AttachOptions)(4));
            w8.YOptions = ((global::Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label4 = new global::Gtk.Label ();
            this.label4.Name = "label4";
            this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("[[login-language]]");
            this.table1.Add (this.label4);
            global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
            w9.TopAttach = ((uint)(3));
            w9.BottomAttach = ((uint)(4));
            w9.XOptions = ((global::Gtk.AttachOptions)(4));
            w9.YOptions = ((global::Gtk.AttachOptions)(4));
            this.vbox1.Add (this.table1);
            global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.table1]));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.checkbutton1 = new global::Gtk.CheckButton ();
            this.checkbutton1.CanFocus = true;
            this.checkbutton1.Name = "checkbutton1";
            this.checkbutton1.Label = global::Mono.Unix.Catalog.GetString ("[[login-ssl]]");
            this.checkbutton1.DrawIndicator = true;
            this.checkbutton1.UseUnderline = true;
            this.vbox1.Add (this.checkbutton1);
            global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.checkbutton1]));
            w11.Position = 2;
            w11.Expand = false;
            w11.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.progressbar1 = new global::Gtk.ProgressBar ();
            this.progressbar1.Name = "progressbar1";
            this.vbox1.Add (this.progressbar1);
            global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.progressbar1]));
            w12.Position = 3;
            w12.Expand = false;
            w12.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.label5 = new global::Gtk.Label ();
            this.label5.Name = "label5";
            this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("label5");
            this.vbox1.Add (this.label5);
            global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label5]));
            w13.Position = 4;
            w13.Expand = false;
            w13.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.button1 = new global::Gtk.Button ();
            this.button1.CanFocus = true;
            this.button1.Name = "button1";
            this.button1.UseUnderline = true;
            this.button1.Label = global::Mono.Unix.Catalog.GetString ("GtkButton");
            this.vbox1.Add (this.button1);
            global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.button1]));
            w14.Position = 5;
            w14.Expand = false;
            w14.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox1 = new global::Gtk.HBox ();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox2 = new global::Gtk.VBox ();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.label6 = new global::Gtk.Label ();
            this.label6.WidthRequest = 280;
            this.label6.Name = "label6";
            this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Translate to other languages");
            this.vbox2.Add (this.label6);
            global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.label6]));
            w15.Position = 0;
            w15.Expand = false;
            w15.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.label7 = new global::Gtk.Label ();
            this.label7.Name = "label7";
            this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("About huggle");
            this.vbox2.Add (this.label7);
            global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.label7]));
            w16.Position = 1;
            w16.Expand = false;
            w16.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.label8 = new global::Gtk.Label ();
            this.label8.Name = "label8";
            this.label8.LabelProp = global::Mono.Unix.Catalog.GetString ("Proxy settings");
            this.vbox2.Add (this.label8);
            global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.label8]));
            w17.Position = 2;
            w17.Expand = false;
            w17.Fill = false;
            this.hbox1.Add (this.vbox2);
            global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
            w18.Position = 0;
            w18.Expand = false;
            w18.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button2 = new global::Gtk.Button ();
            this.button2.WidthRequest = 80;
            this.button2.CanFocus = true;
            this.button2.Name = "button2";
            this.button2.UseUnderline = true;
            this.button2.Label = global::Mono.Unix.Catalog.GetString ("[[exit]]");
            this.hbox1.Add (this.button2);
            global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.button2]));
            w19.Position = 1;
            w19.Expand = false;
            w19.Fill = false;
            this.vbox1.Add (this.hbox1);
            global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
            w20.Position = 6;
            w20.Expand = false;
            w20.Fill = false;
            this.Add (this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll ();
            }
            this.DefaultWidth = 381;
            this.DefaultHeight = 479;
            this.Show ();
        }
    }
}
