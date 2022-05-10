using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentHandlerLibrary.ScintillaAttachmentHelper;

namespace ComponentHandlerLibrary
{
    public class OpenTabButtonComponent : Button
    {
        private TabControl tabControl;
        public OpenTabButtonComponent(TabControl tabcontrol, Point location, Size size)
        {
            this.tabControl = tabcontrol;

            ComponentCollections.OpenTabButtonComponentCollection.Add(this);

            CreateButon(location, size);
            AppendEvents();
            BringToFront();
        }

        ~OpenTabButtonComponent()
        {
            Console.WriteLine($"Object {this}: finalizer is executing.");
        }

        private void CreateButon(Point location, Size size)
        {
            OpenTabButtonComponent button = this;

            button.BackColor = System.Drawing.Color.Black;
            button.ForeColor = System.Drawing.Color.Black;
            button.Name = $"button{ComponentCollections.CloseTabButtonComponentCollection.Count}";
            button.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
            button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button.Location = location;
            button.Size = size;
            button.Text = "New Tab";
            button.Font = new Font(button.Font.Name, 8.0f, button.Font.Style, button.Font.Unit);

            button.UseVisualStyleBackColor = true;

            //Application.OpenForms[0] is the same as referencing mainForm
            Application.OpenForms[0].Controls.Add(button);
        }

        private void Update(int multiplier = 0)
        {
            //Some logic to determine the placement of the button. This was painful to make.
            var thisTabCountIndex = ComponentCollections.TabComponentCollection.Count + multiplier;
            Console.WriteLine(thisTabCountIndex);
            var movePosPerTab = 48 * (thisTabCountIndex + 1);
            var tabPos = Point.Add(new Point((10 + movePosPerTab - 20 + 15), 92), new Size(0, 0));

            //Sets the button's location to the new location.
            this.Location = tabPos;
        }

        #region Events
        //Appends our listener functions to the events
        private void AppendEvents()
        {
            this.Click += event_onClickRemoveComponentAndParent;
            //tabControl.ControlRemoved += even_TabControl_TabIndexChanged_UpdateButton;
            this.tabControl.ControlAdded += even_TabControl_TabIndexChanged_UpdateButtonAdded;
            this.tabControl.ControlRemoved += even_TabControl_TabIndexChanged_UpdateButtonRemoved;
        }

        private void even_TabControl_TabIndexChanged_UpdateButtonAdded(object sender, EventArgs e)
        {
            Console.WriteLine("Tab index changed!!");
            Update();
        }

        private void even_TabControl_TabIndexChanged_UpdateButtonRemoved(object sender, EventArgs e)
        {
            Console.WriteLine("Tab index changed!!");
            Update(-1);
        }

        private void event_onClickRemoveComponentAndParent(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked!");

            new TabComponent(this.tabControl, new CloseTabButtonComponent(),new ScintillaComponent());
        }
        #endregion
    }

}
