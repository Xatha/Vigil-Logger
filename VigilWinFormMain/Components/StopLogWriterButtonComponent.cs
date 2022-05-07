using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentHandlerLibrary;
using ComponentHandlerLibrary.Utils.Components;
using ScintillaNET;

namespace VigilWinFormMain.Components
{
    public class StopLogWriterButtonComponent : ButtonComponentBase
    {
        private readonly TabControl parentTabControl;
        public StopLogWriterButtonComponent(TabControl ParentTabControl)
            : base(Color.Black, Color.Black, new Point(650, 35), new Size(100, 23), "ClearButton", "Stop Logging", AnchorStyles.Top | AnchorStyles.Right)
        {
            this.parentTabControl = ParentTabControl;

            SetEvents();
        }

        protected override void Destroy()
        {
            this.Dispose();
        }

        protected override void SetEvents()
        {
            this.parentTabControl.Selected += event_ParentTabControl_Selecting_ChangeText;
     
            this.Click += event_StopLogWriterButtonComponent_Click_StopLogWriterComponent;
        }

        //Fix this later
        private void event_ParentTabControl_Selecting_ChangeText(object sender, TabControlEventArgs e)
        {
            var LogWriterComponents = TabComponentUtils.GetControlsOfTypeFromSelectedTab<ScintillaLogWriterSubComponent>();

            foreach (var LogWriterComponent in LogWriterComponents)
            {
                if (LogWriterComponent.WritingToken.IsCancellationRequested)
                {
                    this.Text = "Resume Logging";
                }
                else
                {
                    this.Text = "Stop Logging";
                }
            }
        }

        private void event_StopLogWriterButtonComponent_Click_StopLogWriterComponent(object sender, EventArgs e)
        {
            var LogWriterComponents = TabComponentUtils.GetControlsOfTypeFromSelectedTab<ScintillaLogWriterSubComponent>();

            foreach (var LogWriterComponent in LogWriterComponents)
            {
                if (!LogWriterComponent.WritingToken.IsCancellationRequested)
                {
                    LogWriterComponent.StopWriting();
                }
            }
        }


        protected override void ClearEvents()
        {
            throw new NotImplementedException();
        }


    }
}
