using System.Drawing;
using System.Windows.Forms;

namespace ComponentHandlerLibrary
{
    public abstract class ButtonComponentBase : Button
    {
        public virtual TabComponent ParentTabComponent { get; set; }

        public ButtonComponentBase(Color backColor, Color ForeColor, Point location, Size size, string name, string buttonText, AnchorStyles anchor)
        {
            CreateButton(backColor, ForeColor, location, size, name, buttonText, anchor);
            SetEvents();
        }
        protected virtual void CreateButton(Color backColor, Color ForeColor, Point location, Size size, string name, string buttonText, AnchorStyles anchor)
        {
            var button = this;

            button.BackColor = backColor;
            button.ForeColor = ForeColor;
            button.UseVisualStyleBackColor = true;

            button.Anchor = anchor;
            button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            button.Location = location;
            button.Size = size;

            button.Name = name;
            button.Text = buttonText;
            Application.OpenForms[0].Controls.Add(button);
        }
        protected virtual void UpdateButton() { }
        protected abstract void SetEvents();
        protected abstract void ClearEvents();
        protected abstract void Destroy();
    }
}
