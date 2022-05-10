using System.Drawing;

namespace ComponentHandlerLibrary.Utils
{
    public static class Util
    {
        //Converts hexadecimal to color.
        public static Color IntToColor(int rgb) => Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
    }
}
