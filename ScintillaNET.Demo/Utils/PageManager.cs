using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScintillaNET.Demo.Utils
{
    public class LogTab
    {
        private static int TabCount = 1;
        private static bool FirstInit = true;
        private static TabControl tabControl;

        private readonly TabPage tabPageOrig;
        private readonly Scintilla scintillaOrig;
        private Logger logger;
        private string filePath;

        public Scintilla Scintilla { get; }
        public TabPage TabPage { get; }

        public LogTab(TabControl tabControlOrig, TabPage tabPageOrig, Scintilla scintillaOrig, string filePath = null)
        {
            this.filePath = filePath;
            
            this.tabPageOrig = tabPageOrig;
            this.scintillaOrig = scintillaOrig;
            this.Scintilla = new Scintilla();
            this.TabPage = new TabPage();

            if (FirstInit == true)
            {
                tabControl = tabControlOrig;
                this.TabPage = tabPageOrig;
                OnFirstInit();
                OnFirstInit_Events();
                InitStyle();
                
                return;
            }

            InitObjects();
            InitEvents();
            InitStyle();

        }

        #region First Init
        private void OnFirstInit()
        {
            FirstInit = false;

            var scintilla = this.Scintilla;

            //Add properties
            scintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top 
                                | System.Windows.Forms.AnchorStyles.Bottom)
                                | System.Windows.Forms.AnchorStyles.Left)
                                | System.Windows.Forms.AnchorStyles.Right)));
            scintilla.Location = new System.Drawing.Point(-4, 0);
            scintilla.Name = "scintilla1";
            scintilla.Size = new System.Drawing.Size(769, 516);
            scintilla.TabIndex = 0;
            scintilla.Text = "first object :D";

            //Add objects APP.
            tabPageOrig.Controls.Add(scintilla);
            TabCount++;
        }

        private void OnFirstInit_Events()
        {
          
        }
        #endregion

        private void InitObjects()
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

        #region Events
        private void InitEvents()
        {
            tabControl.Selected += TabControl_Selected;
            tabControl.Deselected += TabControl_Deselected;
        }

        private void TabControl_Deselected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == this.TabPage)
            {
                MessageBox.Show("You deselected tab: " + sender.ToString());
                logger.Pause();
                return;
            }

        }

        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == this.TabPage)
            {
                MessageBox.Show("You selected tab: " + TabPage.Name);
                logger.Resume();
                return;

            }

        }
        #endregion

        //Sets style colorcoding for the object's Scintilla.  
        private void InitStyle()
        {
            this.Scintilla.StyleResetDefault();
            this.Scintilla.Styles[Style.Default].Font = "Consolas";
            this.Scintilla.Styles[Style.Default].Size = 10;
            this.Scintilla.Styles[Style.Default].BackColor = Util.IntToColor(0x212121);
            this.Scintilla.Styles[Style.Default].ForeColor = Util.IntToColor(0xFFFFFF);

            this.Scintilla.CaretForeColor = Util.IntToColor(0xFFFFFF);
            this.Scintilla.StyleClearAll();

            this.Scintilla.Styles[LoggingLexer.StyleDefault].ForeColor = Color.White;
            this.Scintilla.Styles[LoggingLexer.StyleDebug].ForeColor = Color.Orange;

            this.Scintilla.Styles[LoggingLexer.StyleInfo].ForeColor = Color.LightGray;
            this.Scintilla.Styles[LoggingLexer.StyleMessage].ForeColor = Color.SlateGray;
            this.Scintilla.Styles[LoggingLexer.StyleWarning].ForeColor = Color.Yellow;
            this.Scintilla.Styles[LoggingLexer.StyleError].ForeColor = Color.Red;
            this.Scintilla.Styles[LoggingLexer.StyleTime].ForeColor = Color.Green;
            this.Scintilla.Lexer = Lexer.Container;

            LoggingLexer loggingLexer = new LoggingLexer(Scintilla);

            Scintilla.StyleNeeded += (s, se) =>
            {
                var startPos = Scintilla.GetEndStyled();
                var endPos = se.Position;

                loggingLexer.Style(Scintilla, startPos, endPos);
            };
        }

        public void StartWrite()
        {
            logger = new Logger(new TextManager(Scintilla), new ConsoleLogReader(this.filePath));
        }
    }
}
