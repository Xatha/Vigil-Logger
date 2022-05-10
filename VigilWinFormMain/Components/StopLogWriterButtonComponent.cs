using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ComponentHandlerLibrary;
using ComponentHandlerLibrary.Utils.Components;

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

        protected override void Destroy() => Dispose();

        protected override void SetEvents()
        {
            this.Click += event_StopLogWriterButtonComponent_Click_StopLogWriterComponent;
        }


        private void event_StopLogWriterButtonComponent_Click_StopLogWriterComponent(object sender, EventArgs e)
        {
            List<ScintillaLogWriterSubComponent> LogWriterComponents = TabComponentUtils.GetControlsOfTypeFromSelectedTab<ScintillaLogWriterSubComponent>();

            foreach (ScintillaLogWriterSubComponent LogWriterComponent in LogWriterComponents)
            {
                if (!LogWriterComponent.WritingToken.IsCancellationRequested)
                {
                    LogWriterComponent.StopWriting();
                }
            }
        }


        protected override void ClearEvents() => throw new NotImplementedException();


    }
}
