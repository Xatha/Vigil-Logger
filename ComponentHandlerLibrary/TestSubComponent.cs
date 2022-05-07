using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentHandlerLibrary.ScintillaAttachmentHelper;
using ScintillaNET;

namespace ComponentHandlerLibrary
{
    public class TestSubComponent : ScintillaSubComponentBase
    {
        public TestSubComponent()
        {
            Console.WriteLine(this + " Has been created!");
        }

        public override Scintilla ScintillaParent { get; set; }

        public override void Destroy()
        {
            Console.WriteLine(this + " Has been destroyed!");
        }
    }
}
