using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ComponentHandlerLibrary
{
    public class TabComponent : TabPage
    {
        private TabControl tabControl;
        private CloseTabButtonComponent closeTabButton;

        internal List<object> childrenObjectsComponents = new List<object>();

        public TabComponent(TabControl tabControl)
        {
            //Determines which control our TabComponent gets attached to.
            this.tabControl = tabControl;

            //Order matters; firstly our TabComponent is created and added to the control, then our close tab button is added, and lastly our events. 
            this.CreateTab();
            this.CreateCloseTabButton();
            this.AppendEvents();

            this.BackColor = Color.AliceBlue;
            //Add our TabComponent to the collection. 
            ComponentCollections.TabComponentCollection.Add(this);
         }
        ~TabComponent()
        {
            Console.WriteLine($"Object {this.ToString()}: finalizer is executing.");
        }

        //Create and add the Tab to the TabControl
        private void CreateTab()
        {
            //Create Tab Object
            var tabPage = this;
            var tabCount = ComponentCollections.TabComponentCollection.Count;

            //Settings
            tabPage.Name = $"TabComponent{tabCount}";
            tabPage.Text = $"Log {tabCount}";
            tabPage.UseVisualStyleBackColor = true;

            //Add objects to control.
            tabControl.Controls.Add(tabPage);
        }

        //Create the CloseTabButton which is "attached" to the TabComponent
        private void CreateCloseTabButton()
        {
            //Some logic to determine the placement of the button. This was painful to make.
            var tabCount = ComponentCollections.TabComponentCollection.Count;
            var movePosPerTab = 48 * (tabCount + 1);
            var tabPos = Point.Add(new Point((10 + movePosPerTab - 20), 91), new Size(0, 0));
            closeTabButton = new CloseTabButtonComponent(tabPos, new Size(20, 25), this);
            //Hides button, because we only want to display the buton when mouse is held over the tab
            closeTabButton.Hide();
            closeTabButton.BringToFront();
            this.Add(closeTabButton);
        }

        private void UpdateCloseTabButton()
        {
            //Some logic to determine the placement of the button. This was painful to make.
            var thisTabCountIndex = ComponentCollections.TabComponentCollection.IndexOf(this);
            var movePosPerTab = 48 * (thisTabCountIndex + 1);
            var tabPos = Point.Add(new Point((10 + movePosPerTab - 20), 91), new Size(0, 0));

            //Sets the button's location to the new location.
            closeTabButton.Location = tabPos;
        }

        //Update the button locations and TabComponent names.
        public new void Update()
        {
            //Update Button
            UpdateCloseTabButton();

            //Updates names.
            for (int i = 0; i < ComponentCollections.TabComponentCollection.Count; i++)
            {
                ComponentCollections.TabComponentCollection[i].Name = $"TabComponent{i}";
                ComponentCollections.TabComponentCollection[i].Text = $"Log {i}";
            }

        }

        public void Add(params object[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is ScintillaComponent || objects[i] is ScintillaLogWriterComponent)
                {
                    Console.WriteLine($"Added object {objects[0]} to the list.");

                    this.Controls.Add((Control)objects[i]);
                    childrenObjectsComponents.Add(objects[i]);
                }
                else if (objects[i] is CloseTabButtonComponent)
                {
                    Console.WriteLine($"Added object {objects[0]} to the list.");

                    childrenObjectsComponents.Add(objects[i]);
                }
                else
                {
                    Console.WriteLine($"ERROR ADDING {objects[i]} TO {this}");
                }
            }
        }

        //Destroy our object.
        public void Destroy()
        {
            //Removes all references from the list.
            childrenObjectsComponents = null;

            //Remove TabComponent from list.
            ComponentCollections.TabComponentCollection.Remove(this);

            //Remove TabComponent from tabControl.
            tabControl.Controls.Remove(this);

            //Remove events we created.
            TruncateEvents();

            //Dispose of TabComponent. 
            this.Dispose(true);
        }



        #region Events
        //Appends our listener functions to the events. 
        private void AppendEvents()
        {
            tabControl.MouseMove += event_TabControl_MouseMove_HandleCloseTabButton;
            tabControl.ControlRemoved += event_TabControl_ControlRemoved_UpdateTabs;
        }

        //Removes our listeners function. 
        private void TruncateEvents()
        {
            tabControl.MouseMove -= event_TabControl_MouseMove_HandleCloseTabButton;
            tabControl.ControlRemoved -= event_TabControl_ControlRemoved_UpdateTabs;
        }

        private void event_TabControl_ControlRemoved_UpdateTabs(object sender, EventArgs e)
        {
            Update();
        }

        //Makes sure that when we hover our mouse over the current tab and its space, the close button shows up. 
        //And if the inverse is true, then hide the button again. 
        private void event_TabControl_MouseMove_HandleCloseTabButton(object sender, MouseEventArgs e)
        {
            var thisTabCountIndex = ComponentCollections.TabComponentCollection.IndexOf(this);

            //This makes sure that when our TabComponent and CloseTabButtonComponent gets removed and disposed of, that the program wont crash from
            //trying to reference parts of the list that does not exist anymore.
            if (thisTabCountIndex < 0)
            {
                tabControl.MouseMove -= event_TabControl_MouseMove_HandleCloseTabButton;
                Console.WriteLine("No index could be found." + thisTabCountIndex);
                return;
            }

            //If the mouse is hovering over the tab area, and the selected tab is this object, then show the button.
            //Else, if mouse is not hovering over the selected tab, hide it again.
            if ((tabControl.GetTabRect(thisTabCountIndex).Contains(e.Location)) && (tabControl.SelectedTab == this))
            {
                closeTabButton.Show();
            }

            if (!(tabControl.GetTabRect(thisTabCountIndex).Contains(e.Location)) && (tabControl.SelectedTab == this) || !(tabControl.SelectedTab == this))
            {
                closeTabButton.Hide();
            }

        }
        #endregion
    }
}
