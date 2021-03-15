using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outils.Logging
{
    public class CyclicXmlTraceListener : CyclicTextLogger
    {
        protected override void WriteEvent(TraceEventDescriptor eventDescr)
        {
            if (!Initialize())
                return;

            try
            {
                InternalWrite(eventDescr.ToXml());
            }
            catch (Exception ex)
            {
                try
                {
                    Close();
                }
                catch { }

                LogSystemError("Failed to write trace event to cyclic log: {0}", ex.Message);
            }
        }
    }
}
