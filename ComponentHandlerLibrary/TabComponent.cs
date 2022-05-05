using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;
using UtilityLibrary;

namespace ComponentHandlerLibrary
{
    public class TabComponent : TabPage
    {
        private TabControl tabControl;
        internal List<object> childrenObjectsComponents = new List<object>();

        public TabComponent(TabControl tabControl)
        {
            //Determines which control our TabComponent gets attached to.
            this.tabControl = tabControl;

            //Order matters; firstly our TabComponent is created and added to the control, then our close tab button is added, and lastly our events. 
            this.CreateTab();
            //this.CreateCloseTabButton();
            this.AppendEvents();

            //Add our TabComponent to the collection. 
            ComponentCollections.TabComponentCollection.Add(this);
        }

        public TabComponent(TabControl tabControl, params object[] childComponents)
        {
            //Determines which control our TabComponent gets attached to.
            this.tabControl = tabControl;

            //Order matters; firstly our TabComponent is created and added to the control, then our close tab button is added, and lastly our events. 
            this.CreateTab();
            //this.CreateCloseTabButton();
            this.AppendEvents();

            //Add our TabComponent to the collection. 
            ComponentCollections.TabComponentCollection.Add(this);

            //Add child components the object.
            Add(childComponents);
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

        //Update the button locations and TabComponent names.
        public new virtual void Update()
        {
            //Updates names.
            for (int i = 0; i < ComponentCollections.TabComponentCollection.Count; i++)
            {
                ComponentCollections.TabComponentCollection[i].Name = $"TabComponent{i}";
                ComponentCollections.TabComponentCollection[i].Text = $"Log {i}";
            }

            //Update is actually called on the object being destroyed, which will reseult in object reference exception.
            //So, were using try-catch to make sure we only run code that references the current object (or children) does not crash the program.
            try
            {
                //Calls closeTabButtonComponent update - which updates its location.
                var closeTabButton = childrenObjectsComponents.FindFirstOfType<CloseTabButtonComponent>();
                closeTabButton.Update();
            }
            catch
            {
                //Do nothing. Catching here is expected.
            }
        }

        public virtual void Add(params object[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is ScintillaComponent || objects[i] is ScintillaLogWriterComponent || objects[i] is Scintilla)
                {
                    Console.WriteLine($"Added object {objects[i]} to the list.");

                    this.Controls.Add((Control)objects[i]);
                    childrenObjectsComponents.Add(objects[i]);
                }
                else if (objects[i] is CloseTabButtonComponent)
                {
                    var closeTabButtonComponent = (CloseTabButtonComponent)objects[i];
                    closeTabButtonComponent.tabComponent = this;

                    Console.WriteLine($"Added object {objects[i]} to the list.");
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
            var closeTabButton = childrenObjectsComponents.FindFirstOfType<CloseTabButtonComponent>();

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
            try
            {
                if ((tabControl.GetTabRect(thisTabCountIndex).Contains(e.Location)) && (tabControl.SelectedTab == this))
                {
                    closeTabButton.Show();
                }

                if (!(tabControl.GetTabRect(thisTabCountIndex).Contains(e.Location)) && (tabControl.SelectedTab == this) || !(tabControl.SelectedTab == this))
                {
                    closeTabButton.Hide();
                }

            }
            catch
            {

            }
        }
        #endregion
    }
}
