using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentHandlerLibrary.Utils.Button;
using ScintillaNET;

namespace ComponentHandlerLibrary
{
    public class ClearScintillaButtonComponent : ButtonComponentBase
    {
        public ClearScintillaButtonComponent() : base(Color.Black, Color.Black, new Point(677, 65), new Size(75, 23), "ClearButton", "Clear Text", AnchorStyles.Top | AnchorStyles.Right)
        {
           
        }

        protected override void Destroy()
        {
            ClearEvents();
            this.Dispose();
        }

        private void ClearScintilla(Scintilla scintilla)
        {
            scintilla.ClearAll();
        }


        protected override void SetEvents()
        {
            this.Click += event_ClearScintillaButtonComponent_Click_ClearSelectedTabContent;
        }

        private void event_ClearScintillaButtonComponent_Click_ClearSelectedTabContent(object sender, EventArgs e)
        {
            foreach (var tabComponent in ComponentCollections.TabComponentCollection)
            {
                if (tabComponent.IsSelected == true)
                {
                    var sctinllaChildren = tabComponent.childrenObjectsComponents.OfType<Scintilla>();

                    foreach (var sctinllaChild  in sctinllaChildren)
                    {
                        sctinllaChild.ClearAll();
                    }
                }
            }
        }

        protected override void ClearEvents()
        {
            throw new NotImplementedException();
        }
    }
}
