using System;
using System.Linq;
using System.Windows.Forms;
using ComponentHandlerLibrary;
using ComponentHandlerLibrary.ScintillaAttachmentHelper;
using ComponentHandlerLibrary.Utils.Components;
using ScintillaNET;

namespace VigilWinFormMain.Components
{
    public class LogWriterTextBoxFilePath : MetroFramework.Controls.MetroTextBox
    {
        public LogWriterTextBoxFilePath()
        {
            CreateTextBox();
            Application.OpenForms[0].Controls.Add(this);
            AppendEvents();
        }

        private void CreateTextBox()
        {
            this.Location = new System.Drawing.Point(242, 64);
            this.Name = "metroTextBox1";
            this.Size = new System.Drawing.Size(335, 23);
            this.Text = "Enter File Path Here";
        }

        private void PushFilePathToLogWriter(string filePath)
        {
            TabComponent currentlySelectedTab = TabComponentUtils.GetCurrentlySelectedTabComponent();
            ComponentDestructionHandler.DestroySubComponentofType<ScintillaLogWriterSubComponent>(currentlySelectedTab);

            currentlySelectedTab.Add(new ScintillaAttachmentBinder(currentlySelectedTab.ChildrenObjectsComponents.OfType<Scintilla>().First(),
                                     new ScintillaLogWriterSubComponent(@filePath)));

        }



        private void AppendEvents() => this.KeyDown += LogWriterTextBoxFilePath_KeyDown_OnEnter;


        private void LogWriterTextBoxFilePath_KeyDown_OnEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PushFilePathToLogWriter(this.Text);
            }
        }
    }
}
