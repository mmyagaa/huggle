
// This file has been generated by the GUI designer. Do not modify.
namespace Stetic
{
    internal class Gui
    {
        private static bool initialized;
        
        internal static void Initialize (Gtk.Widget iconRenderer)
        {
            if ((Stetic.Gui.initialized == false)) {
                Stetic.Gui.initialized = true;
            }
        }
    }
    
    internal class BinContainer
    {
        private Gtk.Widget child;
        private Gtk.UIManager uimanager;
        
        public static BinContainer Attach (Gtk.Bin bin)
        {
            BinContainer bc = new BinContainer ();
            bin.SizeRequested += new Gtk.SizeRequestedHandler (bc.OnSizeRequested);
            bin.SizeAllocated += new Gtk.SizeAllocatedHandler (bc.OnSizeAllocated);
            bin.Added += new Gtk.AddedHandler (bc.OnAdded);
            return bc;
        }
        
        private void OnSizeRequested (object sender, Gtk.SizeRequestedArgs args)
        {
            if ((this.child != null)) {
                args.Requisition = this.child.SizeRequest ();
            }
        }
        
        private void OnSizeAllocated (object sender, Gtk.SizeAllocatedArgs args)
        {
            if ((this.child != null)) {
                this.child.Allocation = args.Allocation;
            }
        }
        
        private void OnAdded (object sender, Gtk.AddedArgs args)
        {
            this.child = args.Widget;
        }
        
        public void SetUiManager (Gtk.UIManager uim)
        {
            this.uimanager = uim;
            this.child.Realized += new System.EventHandler (this.OnRealized);
        }
        
        private void OnRealized (object sender, System.EventArgs args)
        {
            if ((this.uimanager != null)) {
                Gtk.Widget w;
                w = this.child.Toplevel;
                if (((w != null) 
                    && typeof(Gtk.Window).IsInstanceOfType (w))) {
                    ((Gtk.Window)(w)).AddAccelGroup (this.uimanager.AccelGroup);
                    this.uimanager = null;
                }
            }
        }
    }
    
    internal class ActionGroups
    {
        public static Gtk.ActionGroup GetActionGroup (System.Type type)
        {
            return Stetic.ActionGroups.GetActionGroup (type.FullName);
        }
        
        public static Gtk.ActionGroup GetActionGroup (string name)
        {
            return null;
        }
    }
}
