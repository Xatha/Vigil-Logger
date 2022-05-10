using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentHandlerLibrary.Utils.Button;

namespace ComponentHandlerLibrary
{
    public class CloseTabButtonComponent : Button
    {
        internal TabComponent parentTabComponent { get; set; }

        //Constructor
        public CloseTabButtonComponent(System.Drawing.Point location, System.Drawing.Size size, TabComponent tabComponent)
        {
            this.parentTabComponent = tabComponent;

            ComponentCollections.CloseTabButtonComponentCollection.Add(this);

            CreateButon(location, size);
            BringToFront();
            Hide();
            AppendEvents();
        }
        public CloseTabButtonComponent(System.Drawing.Point location, System.Drawing.Size size)
        {
            this.parentTabComponent = this.parentTabComponent;

            ComponentCollections.CloseTabButtonComponentCollection.Add(this);

            CreateButon(location, size);
            BringToFront();
            Hide();
            AppendEvents();
        }
        public CloseTabButtonComponent()
        {
            var movePosPerTab = 48 * (ComponentCollections.TabComponentCollection.Count + 1);
            var tabPos = Point.Add(new Point((10 + movePosPerTab - 20), 91), new Size(0, 0));

            this.parentTabComponent = this.parentTabComponent;

            ComponentCollections.CloseTabButtonComponentCollection.Add(this);

            CreateButon(tabPos, new Size(20, 25));
            BringToFront();
            Hide();
            AppendEvents();
        }


        ~CloseTabButtonComponent()
        {
            Console.WriteLine($"Object {this}: finalizer is executing.");
        }


        private void CreateButon(Point location, Size size)
        {
            CloseTabButtonComponent button = this;

            button.BackColor = System.Drawing.Color.Black;
            button.ForeColor = System.Drawing.Color.Firebrick;

            button.Name = $"button{ComponentCollections.CloseTabButtonComponentCollection.Count}";
            button.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
            button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button.Location = location;
            button.Size = size;
            button.Text = "x";

            button.UseVisualStyleBackColor = true;

            //Application.OpenForms[0] is the same as referencing mainForm
            Application.OpenForms[0].Controls.Add(button);

        }

        internal new void Update() =>
            //Sets the button's location to the new location.
            this.Location = this.CloseTabButtonPosition();

        //Destroy our object.
        public void Destroy()
        {
            //Remove CloseTabButtonComponent from list.
            ComponentCollections.CloseTabButtonComponentCollection.Remove(this);

            //Remove CloseTabButtonComponent from Application.
            Application.OpenForms[0].Controls.Remove(this);

            //Remove our events.
            TruncateEvents();

            //Dispose of CloseTabButtonComponent.
            Dispose();

        }
        #region Events
        //Appends our listener functions to the events
        private void AppendEvents() => this.Click += event_onClickRemoveComponentAndParent;

        //Removes our listeners function. 
        private void TruncateEvents() => this.Click -= event_onClickRemoveComponentAndParent;

        //Destroy parent and all its atteched objects..
        private void event_onClickRemoveComponentAndParent(object sender, EventArgs e) => ComponentDestructionHandler.DestroyParentAndChildren(this.parentTabComponent);

        #endregion
    }

}
