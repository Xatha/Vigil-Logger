using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentHandlerLibrary.Utils.Components
{
    public static class TabComponentUtils
    {
        public static List<T> GetControlsOfTypeFromSelectedTab<T>()
        {
            List<T> results = new List<T>();

            foreach (var tabComponent in ComponentCollections.TabComponentCollection)
            {
                if (tabComponent.IsSelected)
                {
                    foreach (var childComponent in tabComponent.ChildrenObjectsComponents.OfType<T>())
                    {
                        results.Add(childComponent);
                    }
                }
            }

            return results;
        }

        public static TabComponent GetCurrentlySelectedTabComponent()
        {
            var tabComponents = ComponentCollections.TabComponentCollection;

            foreach (var tabComponent in tabComponents)
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
