using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScintillaNET.Demo.Utils
{
    public class LogTab
    {
        static int TabCount = 2;
        public Scintilla Scintilla { get; }
        public TabPage TabPage { get; }

        private readonly TabControl tabControl;
        private readonly TabPage tabPageOrig;
        private readonly Scintilla scintillaOrig;

        public LogTab(TabControl tabControl, TabPage tabPageOrig, Scintilla scintillaOrig)
        {
            this.tabControl = tabControl;
            this.tabPageOrig = tabPageOrig;
            this.scintillaOrig = scintillaOrig;
            this.Scintilla = new Scintilla();
            this.TabPage = new TabPage();

            Create();
            InitEvents();
        }

        private void Create()
        {
            //Create Tab Object
            //Construct object
            var tabPage = this.TabPage;

            //Add properties
            tabPage.Location = new System.Drawing.Point(4, 24);
            tabPage.Name = tabPage.Name + TabCount.ToString();
            tabPage.Padding = tabPage.Padding;
            tabPage.Size = tabPage.Size;
            tabPage.TabIndex = TabCount;
            tabPage.Text = "Log " + TabCount;
            tabPage.UseVisualStyleBackColor = true;

            //Create Scintilla Object
            //Construct object
            var scintilla = this.Scintilla;

            //Add properties
            scintilla.Anchor = scintillaOrig.Anchor;
            scintilla.Location = scintillaOrig.Location;
            scintilla.Name = (scintillaOrig.Name + TabCount.ToString());
            scintilla.Size = scintillaOrig.Size;
            scintilla.Text = "scintilla" + TabCount.ToString();

            //Add objects APP.
            tabControl.Controls.Add(tabPage);
            tabPage.Controls.Add(scintilla);

            TabCount++;
        }

        private void InitEvents()
        {
            this.TabPage.Enter += new System.EventHandler(this.TabPage_onEnter);
        }
        
        private void TabPage_onEnter(object sender, EventArgs e)
        {
            Scintilla.AppendText("Object hey!");
        }
    }
}
