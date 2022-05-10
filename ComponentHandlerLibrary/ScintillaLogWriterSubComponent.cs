using System;
using System.Threading;
using System.Threading.Tasks;
using ComponentHandlerLibrary.ScintillaAttachmentHelper;
using LogHandlerLibrary;
using ScintillaNET;
using UtilityLibrary.ScintillaUtils;

namespace ComponentHandlerLibrary
{
    public class ScintillaLogWriterSubComponent : ScintillaSubComponentBase
    {
        //Functions as a bool, but for async functions. 
        public CancellationTokenSource WritingToken { get; set; } = new CancellationTokenSource();
        public override Scintilla ScintillaParent { get; set; }

        private bool isDisposing = false;
        private readonly LogReaderHandler reader;

        public ScintillaLogWriterSubComponent(string FilePath)
        {
            this.reader = new LogReaderHandler(FilePath);

            //Being writing to console.
            BeginWriting();
        }

        ~ScintillaLogWriterSubComponent()
        {
            Console.WriteLine($"Object: {this} deconstructor has been called.");
        }

        public override void Destroy()
        {
            //Destroy Attached Objects.
            this.reader.Destroy();
            this.ScintillaParent = null;

            //Stops the writer.
            StopWriting();

            this.isDisposing = true;
            Dispose();
        }

        public void StopWriting()
        {
            //This is really bad and I think it can cause memory leaks.
            if (!this.isDisposing)
            {
                try
                {
                    this.WritingToken.Cancel();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EXPECTED CATCH | POSSIBILITY OF MEMORY LEAK |: ");
                    Console.WriteLine($"{ex.Message} \n {ex.StackTrace}");
                }
            }
        }

        //Outputs lines read from text into Scintilla.
        private async void BeginWriting()
        {
        START:
            while (!this.WritingToken.Token.IsCancellationRequested)
            {
                //Read log lines.
                System.Collections.Generic.List<string> lines = this.reader.GetFileContents();

                //If loglines is null, read from log again.
                if (lines == null)
                {
                    await Task.Delay(10);
                    goto START;
                }

                foreach (var line in lines)
                {
                    if (!this.WritingToken.Token.IsCancellationRequested)
                    {
                        AppendToScintilla(line);
                    }
                    else
                    {
                        break;
                    }
                    await Task.Delay(10);
                }

                await Task.Delay(100);
            }

            this.WritingToken.Dispose();
        }

        //Formats the string and pushes it to the Scintilla.
        private void AppendToScintilla(string logLine)
        {
            //Weird fix, but because of the asynchronous function BeginWriting() it runs before the AttachmentBinder is finished building the components.
            //This means that this function will fire before we get a reference to ScintillaParent.
            if (this.ScintillaParent == null)
            {
                return;
            }

            //Get time, formats it and prints it to log. 
            var currentTime = DateTime.Now.ToString("hh:mm:ss") + ":" + DateTime.Now.Millisecond.ToString().PadRight(3, '0');
            var formattedTime = string.Format("[{0}] ", currentTime);

            //Appends text to console.
            this.ScintillaParent.AppendText(true, formattedTime + logLine);
        }

    }
}


