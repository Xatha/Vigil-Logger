namespace ScintillaNET.Demo
{
    /// <summary>
    /// Object-based text handler. Initilise object in global static space for each Scintilla TextBox you have. 
    /// </summary>
    //Object handles appending text to a Scintilla TextBox. 
    public static class TextManager
    {

        //public TextManager(ScintillaNET.Scintilla Scintilla)
        //{
        //    this.Scintilla = Scintilla;
        //}
        /// <summary>
        /// Appends text to TextBox. Input can be string array, which will be seperated by a whitespace.
        /// </summary>
        public static void AppendText(this Scintilla scintilla, bool newLine = false, params string[] text)
        {
            string joinedString = string.Join(" ", text);

            scintilla.AppendText(joinedString);

            if (newLine == true)
            {
                NewLine(scintilla);
            }

        }
        private static void NewLine(Scintilla scintilla)
        {
            scintilla.AppendText("\r");
        }


    }
}
