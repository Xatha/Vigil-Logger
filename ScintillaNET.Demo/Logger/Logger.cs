using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScintillaNET.Demo
{
    internal class Logger
    {
        private ConsoleLogReader logReader;
        private TextManager TextArea;

        //Pausing. Controlled on an object-basis.  
        private bool isPaused = true;

        public Logger(TextManager TextArea, ConsoleLogReader logReader)
        {
            this.logReader = logReader;
            this.TextArea = TextArea;

            InitLogger();
        }


        //Resumes logging.
        public void Resume()
        {
            isPaused = false;
        }

        //Resumes pauses.
        public void Pause()
        {
            isPaused = true;
        }


        private async void InitLogger()
        {
            
            while (true)
            {
                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> logLines;

                //If paused, pause logging. Saves performance when running multiple objects.
                if (!isPaused)
                {
                    logLines = logReader.GetConfigLog();

                    if (logLines != null)
                    {
                        for (int i = 0; i < logLines.Count; i++)
                        {
                            LoggerFormat(logLines[i].ToString());
                            await Task.Delay(10);
                        }
                    }
                    await Task.Delay(100);
                }
                await Task.Delay(1000);
            }
        }

        //Formats the string and pushes it to the Scintilla.
        private void LoggerFormat(string logLine)
        {
            //Get time, formats it and prints it to log. 
            var currentTime = DateTime.Now.ToString("hh:mm:ss") + ":" + DateTime.Now.Millisecond.ToString().PadRight(3, '0');
            var formattedTime = String.Format("[{0}] ", currentTime);

            //Appends text to console.
            TextArea.AppendText(true, formattedTime + logLine);
        }

    }
}
