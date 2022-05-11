using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ComponentHandlerLibrary;
using ComponentHandlerLibrary.ScintillaAttachmentHelper;
using ComponentHandlerLibrary.Utils;
using ComponentHandlerLibrary.Utils.Button;
using ComponentHandlerLibrary.Utils.Components;
using LexerStyleLibrary.Styles;
using ScintillaNET;
using ScintillaNET.Demo.Utils;
using VigilWinFormMain.Components;

namespace VigilWinFormMain
{
    public partial class MainForm : Form
    {
        private Scintilla TextArea;

        private TabControl TabControlMain { get; set; }

        public MainForm()
        {
            InitializeComponent();
            this.TextArea = new ScintillaNET.Scintilla();


            // new ClearScintillaButtonComponent();

            //var mybutton = new System.Windows.Forms.Button();
            //mybutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //mybutton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            //mybutton.Location = new System.Drawing.Point(677, 65);
            //mybutton.Name = "button1";
            //mybutton.Size = new System.Drawing.Size(75, 50);
            //mybutton.TabIndex = 12;
            //mybutton.Text = "button1";
            //mybutton.UseVisualStyleBackColor = true;
            //this.Controls.Add(mybutton);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            // Addiing Properties
            this.TabControlMain = this.tabControl1;

            // Initialise first-time objects and helpers.
            new TabControlHelper(this.TabControlMain);

            new TabComponent(this.TabControlMain, new ScintillaComponent());

            new ClearScintillaButtonComponent();
            new StopLogWriterButtonComponent(this.TabControlMain);
            new OpenTabButtonComponent(this.TabControlMain, OpenTabButtonUtil.OpenTabButtonPosition(), new Size(75, 25));
            new LogWriterTextBoxFilePath();
            // STYLING
            InitColors();
            InitSyntaxColoring();

            // MARGINS
            InitNumberMargin();
            InitBookmarkMargin();
            InitCodeFolding();

            // DRAG DROP
            InitDragDropFile();

            // DEFAULT FILE
            LoadDataFromFile("../../MainForm.cs");

            // INIT HOTKEYS
            InitHotkeys();

        }

        private void InitColors() => this.TextArea.SetSelectionBackColor(true, Util.IntToColor(0x114D9C));

        private void InitHotkeys()
        {
            // register the hotkeys with the form
            HotKeyManager.AddHotKey(this, OpenSearch, Keys.F, true);
            HotKeyManager.AddHotKey(this, OpenFindDialog, Keys.F, true, false, true);
            HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.R, true);
            HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.H, true);
            HotKeyManager.AddHotKey(this, Uppercase, Keys.U, true);
            HotKeyManager.AddHotKey(this, Lowercase, Keys.L, true);
            HotKeyManager.AddHotKey(this, ZoomIn, Keys.Oemplus, true);
            HotKeyManager.AddHotKey(this, ZoomOut, Keys.OemMinus, true);
            HotKeyManager.AddHotKey(this, ZoomDefault, Keys.D0, true);
            HotKeyManager.AddHotKey(this, CloseSearch, Keys.Escape);

            // remove conflicting hotkeys from scintilla
            this.TextArea.ClearCmdKey(Keys.Control | Keys.F);
            this.TextArea.ClearCmdKey(Keys.Control | Keys.R);
            this.TextArea.ClearCmdKey(Keys.Control | Keys.H);
            this.TextArea.ClearCmdKey(Keys.Control | Keys.L);
            this.TextArea.ClearCmdKey(Keys.Control | Keys.U);

        }

        private void InitSyntaxColoring()
        {

            // Configure the default style
            this.TextArea.StyleResetDefault();
            this.TextArea.Styles[Style.Default].Font = "Consolas";
            this.TextArea.Styles[Style.Default].Size = 10;
            this.TextArea.Styles[Style.Default].BackColor = Util.IntToColor(0x212121);
            this.TextArea.Styles[Style.Default].ForeColor = Util.IntToColor(0xFFFFFF);
            this.TextArea.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            //TextArea.Styles[Style.Cpp.Identifier].ForeColor = Util.IntToColor(0xD0DAE2);
            //TextArea.Styles[Style.Cpp.Comment].ForeColor = Util.IntToColor(0xBD758B);
            //TextArea.Styles[Style.Cpp.CommentLine].ForeColor = Util.IntToColor(0x40BF57);
            //TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = Util.IntToColor(0x2FAE35);
            //TextArea.Styles[Style.Cpp.Number].ForeColor = Util.IntToColor(0xFFFF00);
            //TextArea.Styles[Style.Cpp.String].ForeColor = Util.IntToColor(0xFFFF00);
            //TextArea.Styles[Style.Cpp.Character].ForeColor = Util.IntToColor(0xE95454);
            //TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = Util.IntToColor(0x8AAFEE);
            //TextArea.Styles[Style.Cpp.Operator].ForeColor = Util.IntToColor(0xE0E0E0);
            //TextArea.Styles[Style.Cpp.Regex].ForeColor = Util.IntToColor(0xff00ff);
            //TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = Util.IntToColor(0x77A7DB);
            //TextArea.Styles[Style.Cpp.Word].ForeColor = Util.IntToColor(0x48A8EE);
            //TextArea.Styles[Style.Cpp.Word2].ForeColor = Util.IntToColor(0xF98906);
            //TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = Util.IntToColor(0xB3D991);
            //TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = Util.IntToColor(0xFF0000);
            //TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = Util.IntToColor(0x48A8EE);

            this.TextArea.Styles[LoggingStyle.StyleDefault].ForeColor = Color.Wheat;
            this.TextArea.Styles[LoggingStyle.StyleKeyword].ForeColor = Color.Blue;
            this.TextArea.Styles[LoggingStyle.StyleIdentifier].ForeColor = Color.Teal;
            this.TextArea.Styles[LoggingStyle.StyleNumber].ForeColor = Color.Purple;
            this.TextArea.Styles[LoggingStyle.StyleString].ForeColor = Color.Red;

            this.TextArea.Styles[LoggingStyle.StyleDebug].ForeColor = Color.Orange;

            this.TextArea.Styles[LoggingStyle.StyleInfo].ForeColor = Color.LightGray;
            this.TextArea.Styles[LoggingStyle.StyleMessage].ForeColor = Color.SlateGray;
            this.TextArea.Styles[LoggingStyle.StyleWarning].ForeColor = Color.Yellow;
            this.TextArea.Styles[LoggingStyle.StyleError].ForeColor = Color.Red;
            this.TextArea.Styles[LoggingStyle.StyleTime].ForeColor = Color.Green;
            this.TextArea.Lexer = Lexer.Container;

            //TextArea.SetKeywords(0, "class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            //TextArea.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

            var loggingLexer = new LoggingStyle();
            this.TextArea.StyleNeeded += (s, se) =>
            {
                var startPos = this.TextArea.GetEndStyled();
                var endPos = se.Position;

                loggingLexer.Style(this.TextArea, startPos, endPos);
            };
        }

        private void OnTextChanged(object sender, EventArgs e) =>
            //Auto Scrolling.
            this.TextArea.LineScroll(this.TextArea.Lines.Count, 0);



        #region Numbers, Bookmarks, Code Folding

        /// <summary>
        /// the background color of the text area
        /// </summary>
        private const int BACK_COLOR = 0x2A211C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        private const int FORE_COLOR = 0xB7B7B7;

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        private const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the bookmarks/breakpoints to show in
        /// </summary>
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        private const int FOLDING_MARGIN = 3;

        /// <summary>
        /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
        /// </summary>
        private const bool CODEFOLDING_CIRCULAR = true;

        private void InitNumberMargin()
        {

            this.TextArea.Styles[Style.LineNumber].BackColor = Util.IntToColor(BACK_COLOR);
            this.TextArea.Styles[Style.LineNumber].ForeColor = Util.IntToColor(FORE_COLOR);
            this.TextArea.Styles[Style.IndentGuide].ForeColor = Util.IntToColor(FORE_COLOR);
            this.TextArea.Styles[Style.IndentGuide].BackColor = Util.IntToColor(BACK_COLOR);

            Margin nums = this.TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            this.TextArea.MarginClick += TextArea_MarginClick;
        }

        private void InitBookmarkMargin()
        {

            //TextArea.SetFoldMarginColor(true, Util.IntToColor(BACK_COLOR));

            Margin margin = this.TextArea.Margins[BOOKMARK_MARGIN];
            margin.Width = 20;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1 << BOOKMARK_MARKER);
            //margin.Cursor = MarginCursor.Arrow;

            Marker marker = this.TextArea.Markers[BOOKMARK_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(Util.IntToColor(0xFF003B));
            marker.SetForeColor(Util.IntToColor(0x000000));
            marker.SetAlpha(100);

        }

        private void InitCodeFolding()
        {

            this.TextArea.SetFoldMarginColor(true, Util.IntToColor(BACK_COLOR));
            this.TextArea.SetFoldMarginHighlightColor(true, Util.IntToColor(BACK_COLOR));

            // Enable code folding
            this.TextArea.SetProperty("fold", "1");
            this.TextArea.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            this.TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            this.TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            this.TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
            this.TextArea.Margins[FOLDING_MARGIN].Width = 20;

            // Set colors for all folding markers
            for (var i = 25; i <= 31; i++)
            {
                this.TextArea.Markers[i].SetForeColor(Util.IntToColor(BACK_COLOR)); // styles for [+] and [-]
                this.TextArea.Markers[i].SetBackColor(Util.IntToColor(FORE_COLOR)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            this.TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            this.TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            this.TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            this.TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            this.TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            this.TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            this.TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            this.TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }

        private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == BOOKMARK_MARGIN)
            {
                // Do we have a marker for this line?
                const uint mask = (1 << BOOKMARK_MARKER);
                Line line = this.TextArea.Lines[this.TextArea.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0)
                {
                    // Remove existing bookmark
                    line.MarkerDelete(BOOKMARK_MARKER);
                }
                else
                {
                    // Add bookmark
                    line.MarkerAdd(BOOKMARK_MARKER);
                }
            }
        }

        #endregion

        #region Drag & Drop File

        public void InitDragDropFile()
        {

            this.TextArea.AllowDrop = true;
            this.TextArea.DragEnter += delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            };
            this.TextArea.DragDrop += delegate (object sender, DragEventArgs e)
            {

                // get file drop
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {

                    var a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    if (a != null)
                    {

                        var path = a.GetValue(0).ToString();

                        LoadDataFromFile(path);

                    }
                }
            };

        }

        private void LoadDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                //FileName.Text = Path.GetFileName(path);
                //TextArea.Text = File.ReadAllText(path);
            }
        }

        #endregion

        #region Main Menu Commands

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadDataFromFile(this.openFileDialog.FileName);
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e) => OpenSearch();

        private void findDialogToolStripMenuItem_Click(object sender, EventArgs e) => OpenFindDialog();

        private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e) => OpenReplaceDialog();

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) => this.TextArea.Cut();

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) => this.TextArea.Copy();

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) => this.TextArea.Paste();

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) => this.TextArea.SelectAll();

        private void selectLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Line line = this.TextArea.Lines[this.TextArea.CurrentLine];
            this.TextArea.SetSelection(line.Position + line.Length, line.Position);
        }

        private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e) => this.TextArea.SetEmptySelection(0);

        private void indentSelectionToolStripMenuItem_Click(object sender, EventArgs e) => Indent();

        private void outdentSelectionToolStripMenuItem_Click(object sender, EventArgs e) => Outdent();

        private void uppercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e) => Uppercase();

        private void lowercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e) => Lowercase();

        private void wordWrapToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            // toggle word wrap
            this.wordWrapItem.Checked = !this.wordWrapItem.Checked;
            this.TextArea.WrapMode = this.wordWrapItem.Checked ? WrapMode.Word : WrapMode.None;
        }

        private void indentGuidesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // toggle indent guides
            this.indentGuidesItem.Checked = !this.indentGuidesItem.Checked;
            this.TextArea.IndentationGuides = this.indentGuidesItem.Checked ? IndentView.LookBoth : IndentView.None;
        }

        private void hiddenCharactersToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // toggle view whitespace
            this.hiddenCharactersItem.Checked = !this.hiddenCharactersItem.Checked;
            this.TextArea.ViewWhitespace = this.hiddenCharactersItem.Checked ? WhitespaceMode.VisibleAlways : WhitespaceMode.Invisible;
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e) => ZoomIn();

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e) => ZoomOut();

        private void zoom100ToolStripMenuItem_Click(object sender, EventArgs e) => ZoomDefault();

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) => this.TextArea.FoldAll(FoldAction.Contract);

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) => this.TextArea.FoldAll(FoldAction.Expand);


        #endregion

        #region Uppercase / Lowercase

        private void Lowercase()
        {

            // save the selection
            var start = this.TextArea.SelectionStart;
            var end = this.TextArea.SelectionEnd;

            // modify the selected text
            this.TextArea.ReplaceSelection(this.TextArea.GetTextRange(start, end - start).ToLower());

            // preserve the original selection
            this.TextArea.SetSelection(start, end);
        }

        private void Uppercase()
        {

            // save the selection
            var start = this.TextArea.SelectionStart;
            var end = this.TextArea.SelectionEnd;

            // modify the selected text
            this.TextArea.ReplaceSelection(this.TextArea.GetTextRange(start, end - start).ToUpper());

            // preserve the original selection
            this.TextArea.SetSelection(start, end);
        }

        #endregion

        #region Indent / Outdent

        private void Indent() =>
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
            // although the indentation function exists. Pressing TAB with the editor focused confirms this.
            GenerateKeystrokes("{TAB}");

        private void Outdent() =>
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
            // although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
            GenerateKeystrokes("+{TAB}");

        private void GenerateKeystrokes(string keys)
        {
            HotKeyManager.Enable = false;
            this.TextArea.Focus();
            SendKeys.Send(keys);
            HotKeyManager.Enable = true;
        }

        #endregion

        #region Zoom

        private void ZoomIn() => this.TextArea.ZoomIn();

        private void ZoomOut() => this.TextArea.ZoomOut();

        private void ZoomDefault() => this.TextArea.Zoom = 0;


        #endregion

        #region Quick Search Bar

        private bool SearchIsOpen = false;

        private void OpenSearch()
        {

            SearchManager.SearchBox = this.TxtSearch;
            SearchManager.TextArea = this.TextArea;

            if (!this.SearchIsOpen)
            {
                this.SearchIsOpen = true;
                InvokeIfNeeded(delegate ()
                {
                    this.PanelSearch.Visible = true;
                    this.TxtSearch.Text = SearchManager.LastSearch;
                    this.TxtSearch.Focus();
                    this.TxtSearch.SelectAll();
                });
            }
            else
            {
                InvokeIfNeeded(delegate ()
                {
                    this.TxtSearch.Focus();
                    this.TxtSearch.SelectAll();
                });
            }
        }
        private void CloseSearch()
        {
            if (this.SearchIsOpen)
            {
                this.SearchIsOpen = false;
                InvokeIfNeeded(delegate ()
                {
                    this.PanelSearch.Visible = false;
                    //CurBrowser.GetBrowser().StopFinding(true);
                });
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e) => CloseSearch();

        private void BtnPrevSearch_Click(object sender, EventArgs e) => SearchManager.Find(false, false);
        private void BtnNextSearch_Click(object sender, EventArgs e) => SearchManager.Find(true, false);
        private void TxtSearch_TextChanged(object sender, EventArgs e) => SearchManager.Find(true, true);

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (HotKeyManager.IsHotkey(e, Keys.Enter))
            {
                SearchManager.Find(true, false);
            }
            if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true))
            {
                SearchManager.Find(false, false);
            }
        }

        #endregion

        #region Find & Replace Dialog

        private void OpenFindDialog()
        {

        }
        private void OpenReplaceDialog()
        {


        }

        #endregion
        public void InvokeIfNeeded(Action action)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void FileName_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var scintillaComponents = ComponentCollections.ScintillaComponentCollection;

            if (checkBox1.Checked)
            {
                foreach (var scintillaComponent in scintillaComponents)
                {
                    scintillaComponent.StartAutoscrolling();
                }
            }
            else
            {
                foreach (var scintillaComponent in scintillaComponents)
                {
                    scintillaComponent.StopAutoscrolling();
                }
            }

        }
    }
}
