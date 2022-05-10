using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Windows.Forms;
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
            var trimmedFilePath = FilePath?.Trim('"');

            //If no config file path is given, set to a default path.
            if (IsFilePathValid(trimmedFilePath))
            {
                this.filePath = System.IO.Path.GetFullPath(Path.Combine(trimmedFilePath));
            }
            else
            {
                MessageBox.Show($"File Path could not be resolved. Please check your input");
                Console.WriteLine($"ERROR: FilePath could not be resolved.");
                Destroy();
            }
        }

        public void Destroy()
        {
            this.isDestroying = true;
            this.psInstance.Stop();
            this.psInstance.Dispose();
        }

        //Gets config log by doing some witch-craft with Get-Content. Using this method because locked files are a bitch and this bypasses it. 
        //Better optimised, should look at it again.

        //Totally random string. We just need the string to be initialised for the first iteration of the function for the equality check.
        //If the PSObject is initialised as "", it will not read the file on start, until it is written to.
        private PSObject lastLine = "[2[1][3]x[z]aq[w]r[4]2[12";

        public List<string> GetFileContents()
        {
            if (this.isDestroying)
            {
                return null;
            }

            //Clears the commands
            this.psInstance.Commands.Clear();

            //Add the commands again, with a skiplines parameter to make sure we dont read the whole file again.
            this.psInstance.AddCommand("Get-Content").AddArgument(this.filePath).AddCommand("Select").AddParameter("Skip", this.skipLines);
            // psInstance.AddScript("Get-Content " + configFilePath + " | Select -Skip " + skipLines);

            //The lines that were read by the powerShell is put into this collection.
            System.Collections.ObjectModel.Collection<PSObject> contents = this.psInstance.Invoke();

            if (contents.Count == 0)
            {
                //Not perfect. If there's no lines read, then try going backwards until line is reading again. 
                //Reason for this is, most loggers clear the log file's contents on application restart.
                if (this.skipLines > 0)
                {
                    this.skipLines -= 1;
                }

                return null;
            }

            //If theres any content on the line, then set skipLines to the last line read.
            if (contents != null)
            {
                //If were ending up reading the same lines of code, don't return it.
                //This only check the last line. It is possible that this will unintentionally trip, but that is unlikely.
                if (lastLine.Equals(contents.Last()))
                {
                    return null;
                }

                this.skipLines += contents.Count + 1;
            }
            
            var lines = new List<string>();
            foreach (PSObject line in contents)
            {
                lines.Add(line.ToString());
            }

            if (lines.Contains(lastLine.ToString()))
            {
                lines.Remove(lastLine.ToString());
            }

            lastLine = contents.Last();
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
