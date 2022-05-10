using System.Drawing;
using System.Windows.Forms;

namespace ComponentHandlerLibrary
{
    public abstract class ButtonComponentBase : Button
    {
        public virtual TabComponent ParentTabComponent { get; set; }

        public ButtonComponentBase(Color backColor, Color ForeColor, Point location, Size size, string name, string buttonText, AnchorStyles anchor) => Initialize(backColor, ForeColor, location, size, name, buttonText, anchor);
        protected virtual void Initialize(Color backColor, Color ForeColor, Point location, Size size, string name, string buttonText, AnchorStyles anchor)
        {
            CreateButton(backColor, ForeColor, location, size, name, buttonText, anchor);
            Application.OpenForms[0].Controls.Add(this);
        }

        protected virtual void CreateButton(Color backColor, Color ForeColor, Point location, Size size, string name, string buttonText, AnchorStyles anchor)
        {
            ButtonComponentBase button = this;

            button.BackColor = backColor;
            button.ForeColor = ForeColor;
            button.UseVisualStyleBackColor = true;

            button.Anchor = anchor;
            button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            button.Location = location;
            button.Size = size;

            button.Name = name;
            button.Text = buttonText;
        }
        protected virtual void UpdateButton() { }
        protected abstract void SetEvents();
        protected abstract void ClearEvents();
        protected abstract void Destroy();
    }
}
