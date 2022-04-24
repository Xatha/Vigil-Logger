
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace ScintillaNET.Demo
{
    /// <summary>
    /// Object-based text handler. Initilise object in global static space for each Scintilla TextBox you have. 
    /// </summary>
    //Object handles appending text to a Scintilla TextBox. 
    public class TextManager
    {
        private ScintillaNET.Scintilla TextArea;

        public TextManager(ScintillaNET.Scintilla TextArea)
        {
            this.TextArea = TextArea;
        }

        /// <summary>
        /// Appends text to TextBox. Input can be string array, which will be seperated by a whitespace.
        /// </summary>
        public void AppendText(bool newLine = false, params string[] text)
        {
            string joinedString = string.Join(" ", text);
            
            TextArea.AppendText(joinedString);

            if (newLine == true)
            {
                NewLine();
            }

        }
        private void NewLine()
        {
            TextArea.AppendText("\r");
        }
    }
}
