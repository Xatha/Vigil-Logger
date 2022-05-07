using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var currentlySelectedTab = TabComponentUtils.GetCurrentlySelectedTabComponent();
            ComponentDestructionHandler.DestroySubComponentofType<ScintillaLogWriterSubComponent>(currentlySelectedTab);

            Console.WriteLine(currentlySelectedTab.ChildrenObjectsComponents.OfType<Scintilla>().First());
            currentlySelectedTab.Add(new ScintillaAttachmentBinder(currentlySelectedTab.ChildrenObjectsComponents.OfType<Scintilla>().First(),
                                     new ScintillaLogWriterSubComponent(@filePath) { FilePath = @filePath }));

        }



        private void AppendEvents()
        {
            this.KeyDown += LogWriterTextBoxFilePath_KeyDown_OnEnter;
        }


        private void LogWriterTextBoxFilePath_KeyDown_OnEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Console.WriteLine($"Pushing text: { this.Text }");
                PushFilePathToLogWriter(this.Text);
            }
        }
    }
}
