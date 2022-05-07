using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Windows.Forms;
using static ScintillaNET.Style;
using PowerShell = System.Management.Automation.PowerShell;

namespace LogHandlerLibrary
{
    public class LogReaderHandler
    {
        private readonly PowerShell psInstance = PowerShell.Create();
        private int skipLines = 0;
        private string filePath;
        private bool isDestroying = false;

        public LogReaderHandler(string FilePath = null)
        {
            //Trim FilePath in case it had quotation marks included
            var trimmedFilePath = FilePath != null ? FilePath.Trim('"') : null;

            //If no config file path is given, set to a default path.
            if (IsFilePathValid(trimmedFilePath))
            {
                this.filePath = System.IO.Path.GetFullPath(Path.Combine(trimmedFilePath));
            }
            else
            {
                MessageBox.Show($"File Path could not be resolved. Please check your input");
                Destroy();
            }
        }

        public void Destroy()
        {
            isDestroying = true;
            psInstance.Stop();
            psInstance.Dispose();
        }

        //Gets config log by doing some witch-craft with Get-Content. Using this method because locked files are a bitch and this bypasses it. 
        //Better optimised, should look at it again.
        public List<string> GetFileContents()
        {
            if (isDestroying)
            {
                return null;
            }
            //Clears the commands
            psInstance.Commands.Clear();
            //Add the commands again, with a skiplines parameter to make sure we dont read the whole file again.
            psInstance.AddCommand("Get-Content").AddArgument(filePath).AddCommand("Select").AddParameter("Skip", skipLines);
            // psInstance.AddScript("Get-Content " + configFilePath + " | Select -Skip " + skipLines);

            //The lines that were read by the powerShell is put into this collection.
            var contents = psInstance.Invoke();

            //If file is not in use, reset skipLines.
            if (IsFileInUse(filePath) == false)
            {
                skipLines = 0;
            }

            //If there's no content read, return null.
            if (contents.Count <= 0)
            {
                return null;
            }
            //If theres any content on the line, then set skipLines to the last line read.
            else if (contents != null)
            {
                skipLines += contents.Count;
            }

            //Converts the psObjects into seperate lines and output list of lines.
            List<string> lines = new List<string>();
            foreach (var line in contents)
            {
                lines.Add(line.ToString());
            }
            return lines;
        }

        public bool SetFilePath(string filePath)
        {
            if (IsFilePathValid(@filePath))
            {
                this.filePath = filePath;
                return true;
            }
            else
            {
                Console.WriteLine("test");
                return false;  
            }
        }

        private bool IsFilePathValid(string filePath)
        {
            if (filePath == null || filePath == "" || !File.Exists(filePath))
            {
                return false;
            }

            return true;
        }


        //Checks if the file is in use.
        private static bool IsFileInUse(string sFile)
        {
            try
            {
                //If file exists, empty contents of file.
                //File.WriteAllText(sFile, string.Empty);

                //Can file be opened? (Is it locked?)
                using (FileStream fs = File.Open(sFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXPECTED CATCH: ");
                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
                return true;
            }
            //Otherwise return false.
            return false;
        }

    }

}
