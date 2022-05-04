using System.Collections.Generic;

namespace ComponentHandlerLibrary
{
    public static class ComponentCollections
    {
        public static List<TabComponent> TabComponentCollection { get; set; } = new List<TabComponent>();
        public static List<ScintillaComponent> ScintillaComponentCollection { get; set; } = new List<ScintillaComponent>();
        internal static List<CloseTabButtonComponent> CloseTabButtonComponentCollection { get; set; } = new List<CloseTabButtonComponent>();
    }
}
