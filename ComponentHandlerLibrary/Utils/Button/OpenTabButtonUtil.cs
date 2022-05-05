using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentHandlerLibrary.Utils.Button
{
    public static class OpenTabButtonUtil
    {
        public static Point OpenTabButtonPosition()
        {
            //Some logic to determine the placement of the button. This was painful to make.
            var tabCount = ComponentCollections.TabComponentCollection.Count - 1;
            var movePosPerTab = 48 * (tabCount + 1);

            return Point.Add(new Point((10 + movePosPerTab - 20 + 15), 92), new Size(0, 0));
        }

    }
}
