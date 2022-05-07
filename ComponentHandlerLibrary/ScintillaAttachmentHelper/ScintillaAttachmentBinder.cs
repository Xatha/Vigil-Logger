using System;
using System.Collections.Generic;
using System.Linq;
using ScintillaNET;

namespace ComponentHandlerLibrary.ScintillaAttachmentHelper
{
    public class ScintillaAttachmentBinder
    {
        public Scintilla ScintillaComponent { get; private set; }
        public List<ScintillaSubComponentBase> SubComponents { get; private set; } = new List<ScintillaSubComponentBase>();

        public ScintillaAttachmentBinder(Scintilla scintilla, params ScintillaSubComponentBase[] scintillaSubComponents)
        {
            ScintillaComponent = scintilla;
            AttachSubComponentsToParent(scintilla, scintillaSubComponents);
        }

        private void AttachSubComponentsToParent(Scintilla scintilla, ScintillaSubComponentBase[] scintillaSubComponents)
        {
            foreach (var scintillaSubComponent in scintillaSubComponents)
            {
                SetProperties(scintilla, scintillaSubComponent);
                scintillaSubComponent.ScintillaParent = scintilla;
                Console.WriteLine($"Parent Scintilla for Sub Component {scintillaSubComponent} is {scintillaSubComponent.ScintillaParent}");
            }
        }

        private void SetProperties(Scintilla scintilla, ScintillaSubComponentBase scintillaSubComponent)
        {
            SubComponents.Add(scintillaSubComponent);
        }



    }
}
