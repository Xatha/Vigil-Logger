using System.Drawing;
using System.Windows.Forms;

namespace UtilityLibrary
{
    public static class TabControlUtils
    {
        public static T FindControlFromLocation<T>(this TabControl TabControl, Point location) where T : TabPage
        {
            Control.ControlCollection controlCollection = TabControl.Controls;

            foreach (Control control in controlCollection)
            {
                if (control is T)
                {
                    if (TabControl.GetTabRect(controlCollection.IndexOf(control)).Contains(location))
                    {
                        return control as T;
                    }
                }
            }
            return null;
        }

    }
}
