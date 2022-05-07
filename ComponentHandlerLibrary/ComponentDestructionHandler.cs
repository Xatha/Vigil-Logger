using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentHandlerLibrary
{
    public static class ComponentDestructionHandler
    {
        private static readonly Action<object> DESTRUCTION_MSG = (attachedObject) =>
        {
            Console.WriteLine($"{attachedObject} has been flagged for destruction.");
        };

        public static void DestroyParentAndChildren(TabComponent parentComponent)
        {
            //If it exists, destroy the child objects first. The order matters, since they can depend on each other.
            foreach (var attachedObject in parentComponent.childrenObjectsComponents)
            {
                var childScintillaLogWriterComp = attachedObject as ScintillaLogWriterComponent;
                var childScintillaComp = attachedObject as ScintillaComponent;
                var childCloseTabComp = attachedObject as CloseTabButtonComponent;

                if (childScintillaLogWriterComp != null)
                {
                    DESTRUCTION_MSG(attachedObject);
                    childScintillaLogWriterComp.Destroy();
                }

                if (childScintillaComp != null)
                {
                    DESTRUCTION_MSG(attachedObject);
                    childScintillaComp.Destroy();
                }

                if (childCloseTabComp != null)
                {
                    DESTRUCTION_MSG(attachedObject);
                    childCloseTabComp.Destroy();
                }
            }
            
            //And lastly destroy the parent component.
            DESTRUCTION_MSG(parentComponent);
            parentComponent.Destroy();
        }
    }
}
