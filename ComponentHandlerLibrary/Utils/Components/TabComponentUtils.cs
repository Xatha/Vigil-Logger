using System.Collections.Generic;
using System.Linq;

namespace ComponentHandlerLibrary.Utils.Components
{
    public static class TabComponentUtils
    {
        public static List<T> GetControlsOfTypeFromSelectedTab<T>()
        {
            var results = new List<T>();

            foreach (TabComponent tabComponent in ComponentCollections.TabComponentCollection)
            {
                if (tabComponent.IsSelected)
                {
                    foreach (T childComponent in tabComponent.ChildrenObjectsComponents.OfType<T>())
                    {
                        results.Add(childComponent);
                    }
                }
            }

            return results;
        }

        public static TabComponent GetCurrentlySelectedTabComponent()
        {
            List<TabComponent> tabComponents = ComponentCollections.TabComponentCollection;

            foreach (TabComponent tabComponent in tabComponents)
            {
                if (tabComponent.IsSelected)
                {
                    return tabComponent;
                }
            }

            return null;
        }
    }
}
