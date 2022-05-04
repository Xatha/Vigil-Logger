using System;
using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;

namespace ComponentHandlerLibrary
{
    internal class CloseTabButtonComponent : Button
    {
        private readonly TabComponent tabComponent;
        //Constructor
        internal CloseTabButtonComponent(System.Drawing.Point location, System.Drawing.Size size, TabComponent tabComponent)
        {
            this.tabComponent = tabComponent;

            ComponentCollections.CloseTabButtonComponentCollection.Add(this);

            CreateButon(location, size);
            AppendEvents();
        }

        ~CloseTabButtonComponent()
        {
            Console.WriteLine($"Object {this}: finalizer is executing.");
        }


        private void CreateButon(Point location, Size size)
        {
            var button = this;

            button.BackColor = System.Drawing.Color.Black;
            button.ForeColor = System.Drawing.Color.Firebrick;

            button.Name = $"button{ComponentCollections.CloseTabButtonComponentCollection.Count}";
            button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button.Location = location;
            button.Size = size;
            button.Text = "x";

            button.UseVisualStyleBackColor = true;

            //Application.OpenForms[0] is the same as referencing mainForm
            Application.OpenForms[0].Controls.Add(button);
        }

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
            this.Dispose();

        }

        //Destroy parent and all its atteched objects..
        private void DestroyAll()
        {
            ComponentDestructionHandler.DestroyParentAndChildren(tabComponent);
        }
        #region Events
        //Appends our listener functions to the events
        private void AppendEvents()
        {
            this.Click += event_onClickRemoveComponentAndParent;

        }

        //Removes our listeners function. 
        private void TruncateEvents()
        {
            this.Click -= event_onClickRemoveComponentAndParent;

        }

        private void event_onClickRemoveComponentAndParent(object sender, EventArgs e)
        {
            DestroyAll();
            //this.Destroy();
            //tabComponent.Destroy();
            
        }

        #endregion
    }

}
