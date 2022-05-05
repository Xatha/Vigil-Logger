using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LogHandlerLibrary;
using UtilityLibrary.ScintillaUtils;

namespace ComponentHandlerLibrary
{
    public class ScintillaLogWriterComponent : ScintillaComponent
    {
        private readonly LogReaderHandler reader;
        private bool isDestroying = false;

        public string FilePath { get; set; } = null;
        public ScintillaLogWriterComponent()
        {
            this.reader = new LogReaderHandler(FilePath);

            //For some reason the events arent appended to this component.
            AppendEvents();

            //Being writing to console.
            BeginWriting();
        }
        
        public new void Destroy()
        {
            isDestroying = true;
            //reader.Destroy();
            ////Remove TabComponent from list.
            //ComponentCollections.ScintillaComponentCollection.Remove(this);

            ////Remove events we created.
            //TruncateEvents();

            ////Dispose of TabComponent. 
            //this.Dispose(true);
        }



    //Outputs lines read from text into Scintilla.
    private async void BeginWriting()
        {
            while (!isDestroying)
            {
            START:
                //Read log lines.
                var lines = reader.GetFileContents();

                //If loglines is null, read from log again.
                if (lines == null)
                {
                    goto START;
                }

                foreach (var line in lines)
                {
                    AppendToScintilla(line);

                    await Task.Delay(10);
                }

                await Task.Delay(100);
            }
        }

        //Formats the string and pushes it to the Scintilla.
        private void AppendToScintilla(string logLine)
        {
            //Get time, formats it and prints it to log. 
            var currentTime = DateTime.Now.ToString("hh:mm:ss") + ":" + DateTime.Now.Millisecond.ToString().PadRight(3, '0');
            var formattedTime = String.Format("[{0}] ", currentTime);

            //Appends text to console.
            this.AppendText(true, formattedTime + logLine);
        }

    }
}
