﻿using System;
using System.Collections.Generic;
using ScintillaNET;

namespace ComponentHandlerLibrary.ScintillaAttachmentHelper
{
    public class ScintillaAttachmentBinder
    {
        public Scintilla ScintillaComponent { get; private set; }
        public List<ScintillaSubComponentBase> SubComponents { get; private set; } = new List<ScintillaSubComponentBase>();

        public ScintillaAttachmentBinder(Scintilla scintilla, params ScintillaSubComponentBase[] scintillaSubComponents)
        {
            this.ScintillaComponent = scintilla;
            AttachSubComponentsToParent(scintilla, scintillaSubComponents);
        }

        private void AttachSubComponentsToParent(Scintilla scintilla, ScintillaSubComponentBase[] scintillaSubComponents)
        {
            foreach (ScintillaSubComponentBase scintillaSubComponent in scintillaSubComponents)
            {
                SetProperties(scintilla, scintillaSubComponent);
                scintillaSubComponent.ScintillaParent = scintilla;
            }
        }

        private void SetProperties(Scintilla scintilla, ScintillaSubComponentBase scintillaSubComponent) => this.SubComponents.Add(scintillaSubComponent);



    }
}
