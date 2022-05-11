using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComponentHandlerLibrary.ScintillaAttachmentHelper;
using ScintillaNET;
using UtilityLibrary;

namespace ComponentHandlerLibrary
{
    public class TabComponent : TabPage
    {
        public bool IsSelected { get; private set; } = false;
        public List<object> ChildrenObjectsComponents = new List<object>();

        private TabControl tabControl;

        public TabComponent(TabControl tabControl) => Initialize(tabControl);

        public TabComponent(TabControl tabControl, params object[] childComponents)
        {
            Initialize(tabControl);

            //Add child components the object.
            Add(childComponents);
        }

        ~TabComponent()
        {
            Console.WriteLine($"Object {ToString()}: finalizer is executing.");
        }

        private void Initialize(TabControl tabControl)
        {
            //Determines which control our TabComponent gets attached to.
            this.tabControl = tabControl;

            //Order matters; firstly our TabComponent is created and added to the control, then our close tab button is added, and lastly our events. 
            CreateTab();

            //this.CreateCloseTabButton();
            AppendEvents();

            //Add our TabComponent to the collection. 
            ComponentCollections.TabComponentCollection.Add(this);

            //Makes sure the first tab created on startup is considered selected.
            if (ComponentCollections.TabComponentCollection.Count <= 1)
            {
                this.IsSelected = true;
            }
        }

        //Create and add the Tab to the TabControl
        private void CreateTab()
        {
            //Create Tab Object
            TabComponent tabPage = this;
            var tabCount = ComponentCollections.TabComponentCollection.Count;

            //Settings
            tabPage.Name = $"TabComponent{tabCount}";
            tabPage.Text = $"Log {tabCount}";
            tabPage.UseVisualStyleBackColor = true;

            //Add objects to control.
            this.tabControl.Controls.Add(tabPage);
        }

        //Update the button locations and TabComponent names.
        public new virtual void Update()
        {
            //Updates names.
            for (var i = 0; i < ComponentCollections.TabComponentCollection.Count; i++)
            {
                ComponentCollections.TabComponentCollection[i].Name = $"TabComponent{i}";
                ComponentCollections.TabComponentCollection[i].Text = $"Log {i}";
            }

            //Update is actually called on the object being destroyed, which will reseult in object reference exception.
            //So, were using try-catch to make sure we only run code that references the current object (or children) does not crash the program.
            try
            {
                //Calls closeTabButtonComponent update - which updates its location.
                CloseTabButtonComponent closeTabButton = this.ChildrenObjectsComponents.FindFirstOfType<CloseTabButtonComponent>();
                closeTabButton.Update();
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXPECTED CATCH: ");
                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
            }
        }

        public virtual void Add(params object[] objects)
        {
            for (var i = 0; i < objects.Length; i++)
            {
                if (objects[i] is ScintillaComponent || objects[i] is Scintilla)
                {
                    Console.WriteLine($"Added object {objects[i]} to the list.");

                    this.Controls.Add((Control)objects[i]);
                    this.ChildrenObjectsComponents.Add(objects[i]);
                }
                else if (objects[i] is CloseTabButtonComponent)
                {
                    var closeTabButtonComponent = (CloseTabButtonComponent)objects[i];
                    closeTabButtonComponent.parentTabComponent = this;

                    Console.WriteLine($"Added object {objects[i]} to the list.");
                    this.ChildrenObjectsComponents.Add(objects[i]);
                }
                else if (objects[i] is ScintillaAttachmentBinder)
                {
                    var scintillaAttachmentBinder = objects[i] as ScintillaAttachmentBinder;

                    Console.WriteLine($"Added object {scintillaAttachmentBinder} to the list.");

                    this.ChildrenObjectsComponents.Add(scintillaAttachmentBinder.ScintillaComponent);
                    AddSubComponents(scintillaAttachmentBinder, scintillaAttachmentBinder.SubComponents);

                    this.Controls.Add(scintillaAttachmentBinder.ScintillaComponent);

                }
                else if (objects[i] is ScintillaSubComponentBase)
                {
                    Console.WriteLine($"Adding SubComponent: {objects[i]} TO {this}");
                    this.ChildrenObjectsComponents.Add(objects[i]);
                }
                else
                {
                    Console.WriteLine($"ERROR: COULD NOT ADD UNKNOWN OBJECT: {objects[i]} TO {this}");
                }
            }
        }

        private void AddSubComponents(ScintillaAttachmentBinder attachmentBinder, List<ScintillaSubComponentBase> subComponents)
        {
            foreach (ScintillaSubComponentBase subComponent in subComponents)
            {
                this.ChildrenObjectsComponents.Add(subComponent);
            }
        }

        //Destroy our object.
        public void Destroy()
        {
            //Removes all references from the list.
            this.ChildrenObjectsComponents = null;

            //Remove TabComponent from list.
            ComponentCollections.TabComponentCollection.Remove(this);

            //Remove TabComponent from tabControl.
            this.tabControl.Controls.Remove(this);

            //Remove events we created.
            TruncateEvents();

            //Dispose of TabComponent. 
            Dispose(true);
        }

        #region Events
        //Appends our listener functions to the events. 
        private void AppendEvents()
        {
            this.tabControl.Selected += event_TabControl_Selecte_CheckIfSelectedTabIsThis;
            this.tabControl.MouseMove += event_TabControl_MouseMove_HandleCloseTabButton;
            this.tabControl.ControlRemoved += event_TabControl_ControlRemoved_UpdateTabs;
        }

        private void event_TabControl_Selecte_CheckIfSelectedTabIsThis(object sender, TabControlEventArgs e)
        {
            if (this.tabControl.SelectedTab == this)
            {
                this.IsSelected = true;
            }
            else
            {
                this.IsSelected = false;
            }
        }

        //Removes our listeners function. 
        private void TruncateEvents()
        {
            this.tabControl.MouseMove -= event_TabControl_MouseMove_HandleCloseTabButton;
            this.tabControl.ControlRemoved -= event_TabControl_ControlRemoved_UpdateTabs;
        }

        private void event_TabControl_ControlRemoved_UpdateTabs(object sender, EventArgs e) => Update();

        //Makes sure that when we hover our mouse over the current tab and its space, the close button shows up. 
        //And if the inverse is true, then hide the button again. 
        private void event_TabControl_MouseMove_HandleCloseTabButton(object sender, MouseEventArgs e)
        {
            CloseTabButtonComponent closeTabButton = this.ChildrenObjectsComponents.FindFirstOfType<CloseTabButtonComponent>();

            var thisTabCountIndex = ComponentCollections.TabComponentCollection.IndexOf(this);

            //This makes sure that when our TabComponent and CloseTabButtonComponent gets removed and disposed of, that the program wont crash from
            //trying to reference parts of the list that does not exist anymore.
            if (thisTabCountIndex < 0)
            {
                this.tabControl.MouseMove -= event_TabControl_MouseMove_HandleCloseTabButton;
                Console.WriteLine("No index could be found." + thisTabCountIndex);
                return;
            }

            //If the mouse is hovering over the tab area, and the selected tab is this object, then show the button.
            //Else, if mouse is not hovering over the selected tab, hide it again.
            try
            {
                if ((this.tabControl.GetTabRect(thisTabCountIndex).Contains(e.Location)) && (this.tabControl.SelectedTab == this))
                {
                    closeTabButton.Show();
                }

                if (!(this.tabControl.GetTabRect(thisTabCountIndex).Contains(e.Location)) && (this.tabControl.SelectedTab == this) || !(this.tabControl.SelectedTab == this))
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
