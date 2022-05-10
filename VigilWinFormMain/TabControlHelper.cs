using System.Windows.Forms;
using ComponentHandlerLibrary;
using UtilityLibrary;

namespace VigilWinFormMain
{
    public class TabControlHelper
    {
        private TabControl tabControl;

        public TabControlHelper(TabControl tabControl)
        {
            this.tabControl = tabControl;
            AppendEvents();
        }

        private void AppendEvents()
        {
            this.tabControl.MouseClick += event_TabControl_MouseClick_MiddleClickDestroyTab;

            //Makes it so the selected tab does not deselect when a tab is removed.
            this.tabControl.ControlRemoved += event_TabControl_ControlRemoved_TabSelector;
        }

        private void event_TabControl_ControlRemoved_TabSelector(object sender, ControlEventArgs e) => this.tabControl.SelectTab(this.tabControl.SelectedTab);

        private void event_TabControl_MouseClick_MiddleClickDestroyTab(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                //Get the tabComponent that is clicked on.
                TabComponent tabComponent = this.tabControl.FindControlFromLocation<TabComponent>(e.Location);
                //If the component is the first one, then return. 
                if (tabComponent == ComponentCollections.TabComponentCollection[0])
                {
                    return;
                }
                ComponentDestructionHandler.DestroyParentAndChildren(tabComponent);
            }
        }
    }
}
