using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ComponentHandlerLibrary;
using ComponentHandlerLibrary.Utils.Components;
using ScintillaNET;

namespace VigilWinFormMain.Components
{
    public class ClearScintillaButtonComponent : ButtonComponentBase
    {
        public ClearScintillaButtonComponent()
            : base(Color.Black, Color.Black, new Point(677, 65), new Size(75, 23), "ClearButton", "Clear Text", AnchorStyles.Top | AnchorStyles.Right)
        {
            SetEvents();
        }

        protected override void Destroy()
        {
            ClearEvents();
            this.Dispose();
        }

        protected override void SetEvents()
        {
            this.Click += event_ClearScintillaButtonComponent_Click_ClearSelectedTabContent;
        }

        private void event_ClearScintillaButtonComponent_Click_ClearSelectedTabContent(object sender, EventArgs e)
        {
            Console.WriteLine($"Click!");

            var currentlySelectedTab = TabComponentUtils.GetCurrentlySelectedTabComponent();

            var scintillaChildren = currentlySelectedTab.ChildrenObjectsComponents.OfType<Scintilla>();

            foreach (var scintitillaChild in scintillaChildren)
            {
                scintitillaChild.ClearAll();
            }

        }
        protected override void ClearEvents()
        {
            throw new NotImplementedException();
        }
    }
}

