namespace UtilityLibrary.ScintillaUtils
{
    public static class ScintillaUtils
    {
        /// <summary>
        /// Appends text to TextBox. Input can be string array, which will be seperated by a whitespace.
        /// </summary>
        public static void AppendText(this ScintillaNET.Scintilla scintilla, bool newLine = false, params string[] text)
        {
            var joinedString = string.Join(" ", text);

            scintilla.AppendText(joinedString);

            if (newLine == true)
            {
                NewLine(scintilla);
            }
        }
        private static void NewLine(ScintillaNET.Scintilla scintilla) => scintilla.AppendText("\r");
    }
}
