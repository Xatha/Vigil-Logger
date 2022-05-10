using System;
using System.Linq;
using ComponentHandlerLibrary.ScintillaAttachmentHelper;

namespace ComponentHandlerLibrary
{
    public static class ComponentDestructionHandler
    {
        public static void DestroySubComponentofType<T>(TabComponent parentComponent) where T : ScintillaSubComponentBase
        {
            foreach (var attachedObject in parentComponent.ChildrenObjectsComponents.ToList())
            {
                var childSubComponent = attachedObject as T;

                if (childSubComponent != null)
                {
                    DESTRUCTION_MSG(attachedObject);

                    parentComponent.ChildrenObjectsComponents.Remove(childSubComponent);

                    childSubComponent.Destroy();
                }
            }
        }

        public static void DestroyParentAndChildren(TabComponent parentComponent)
        {
            //If it exists, destroy the child objects first. The order matters, since they can depend on each other.
            foreach (var attachedObject in parentComponent.ChildrenObjectsComponents)
            {
                var childScintillaLogWriterComp = attachedObject as ScintillaSubComponentBase;
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

        private static readonly Action<object> DESTRUCTION_MSG = (attachedObject) =>
        {
            Console.WriteLine($"{attachedObject} has been flagged for destruction.");
        };

    }

}
