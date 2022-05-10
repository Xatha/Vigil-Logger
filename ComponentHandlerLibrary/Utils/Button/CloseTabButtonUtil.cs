using System.Drawing;

namespace ComponentHandlerLibrary.Utils.Button
{
    public static class CloseTabButtonUtil
    {
        public static Point CloseTabButtonPosition(this CloseTabButtonComponent closeTabButtonComponent)
        {
            //Some logic to determine the placement of the button. This was painful to make.
            var thisTabCountIndex = ComponentCollections.TabComponentCollection.IndexOf(closeTabButtonComponent.parentTabComponent);
            var movePosPerTab = 48 * (thisTabCountIndex + 1);
            return Point.Add(new Point((10 + movePosPerTab - 20), 91), new Size(0, 0));
        }
    }
}
