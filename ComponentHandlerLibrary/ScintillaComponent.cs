using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentHandlerLibrary.Utils;
using LexerStyleLibrary.Styles;
using ScintillaNET;

namespace ComponentHandlerLibrary
{
    public class ScintillaComponent : Scintilla
    {
        protected static readonly Point scinLocation = new Point(0, 0);
        protected static readonly Size scinSize = new Size(1200, 1200);
        protected static readonly string scinText = "";
        protected const AnchorStyles scinAnchorStyle = AnchorStyles.Top
                                                                | AnchorStyles.Bottom
                                                                | AnchorStyles.Left
                                                                | AnchorStyles.Right;
        //Constructors
        public ScintillaComponent()
        {
            CreateScintilla(new Point(0, 0), new Size(800, 1200), "", AnchorStyles.Top
                                                                       | AnchorStyles.Bottom
                                                                       | AnchorStyles.Left
                                                                       | AnchorStyles.Right);
            ApplyStyling();
            // BASIC CONFIG
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScrollWidth = 1;
            this.ScrollWidthTracking = true;

            // INITIAL VIEW CONFIG
            this.WrapMode = WrapMode.None;
            this.IndentationGuides = IndentView.LookBoth;
            ComponentCollections.ScintillaComponentCollection.Add(this);
            
        }
        public ScintillaComponent(Point location, Size size, string text, AnchorStyles anchor)
        {
            //Auto scrolling
            this.TextChanged += (sender, e) =>
            {
                this.LineScroll(this.Lines.Count, 0);
            };

            ComponentCollections.ScintillaComponentCollection.Add(this);
        }
        //Deconstructor
        ~ScintillaComponent()
        {
            Console.WriteLine($"ScintillaComponent {ComponentCollections.ScintillaComponentCollection.IndexOf(this)}: Deconstructor has been called.");
        }

        private void CreateScintilla(Point location, Size size, string text, AnchorStyles anchor)
        {
            var scintilla = this;

            //Set the objects parameters.
            scintilla.Name = $"Scintilla{ComponentCollections.ScintillaComponentCollection.Count}";
            scintilla.Location = location;
            scintilla.Size = size;
            scintilla.Text = text;
            scintilla.Anchor = anchor;
        }

        public void Destroy()
        {
            //Remove TabComponent from list.
            ComponentCollections.ScintillaComponentCollection.Remove(this);

            //Remove events we created.
            TruncateEvents();

            //Dispose of TabComponent. 
            this.Dispose(true);
        }

        //Sets style colorcoding for the object's Scintilla.  
        private void ApplyStyling()
        {
            this.StyleResetDefault();
            this.Styles[Style.Default].Font = "Consolas";
            this.Styles[Style.Default].Size = 10;
            this.Styles[Style.Default].BackColor = Util.IntToColor(0x212121);
            this.Styles[Style.Default].ForeColor = Util.IntToColor(0xFFFFFF);

            this.CaretForeColor = Util.IntToColor(0xFFFFFF);
            this.StyleClearAll();

            this.Styles[LoggingStyle.StyleDefault].ForeColor = Color.White;
            this.Styles[LoggingStyle.StyleDebug].ForeColor = Color.Orange;
            this.Styles[LoggingStyle.StyleInfo].ForeColor = Color.LightGray;
            this.Styles[LoggingStyle.StyleMessage].ForeColor = Color.SlateGray;
            this.Styles[LoggingStyle.StyleWarning].ForeColor = Color.Yellow;
            this.Styles[LoggingStyle.StyleError].ForeColor = Color.Red;
            this.Styles[LoggingStyle.StyleTime].ForeColor = Color.Green;
            this.Lexer = Lexer.Container;

            LoggingStyle lexer = new LoggingStyle();

            this.StyleNeeded += (s, se) =>
            {
                var startPos = this.GetEndStyled();
                var endPos = se.Position;

                lexer.Style(this, startPos, endPos);
            };
        }

        #region EVENTS
        protected void AppendEvents()
        {
            //Auto scrolling
            this.TextChanged += event_ScintillaComponent_TextChanged_AutoScroll;
        }

        protected void TruncateEvents()
        {
            this.TextChanged -= event_ScintillaComponent_TextChanged_AutoScroll;

        }

        private void event_ScintillaComponent_TextChanged_AutoScroll(object sender, EventArgs e)
        {
            //Auto scrolling
            this.LineScroll(this.Lines.Count, 0);
            
        }
        #endregion
    }
}
