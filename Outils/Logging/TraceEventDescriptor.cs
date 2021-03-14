using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Outils.Logging
{
    public class TraceEventDescriptor
    {
        /// <summary>
        /// Name of the machine that genrated the trace event
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// ID of the trace event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Type of event
        /// </summary>
        public TraceEventType EventType { get; set; }

        /// <summary>
        /// Gets the current number of ticks in the timer mechanism.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// time of when the trace event was generated.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Id identifiant l'activite qui engloble l'evenement. (pour correlation des logs)
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// process ID of the process who generated this event.
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Name of the process that generated the event
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Thread identifier from where the event was raised.
        /// </summary>
        public string ThreadId { get; set; }

        /// <summary>
        /// When provided, has the call stack up the point where the event was raised.
        /// </summary>
        public string CallStack { get; set; }

        /// <summary>
        /// When available, lists the Logical opperations stack
        /// </summary>
        public string LogicalOperationStack { get; set; }

        /// <summary>
        /// Actuall message describing the trace event.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Indicates the trace source that was used to publish the event
        /// </summary>
        public string TraceSource { get; set; }

        /// <summary>
        /// List of all the activities active at the time the trace was emited.
        /// </summary>
        public string Activities { get; set; }

        #region xml

        /// <summary>
        /// return a string containing an xml representation of the payload of this class.
        /// the xml format is the exact same format of the one produced by the XmlWritterTraceListener
        /// that can be read by Windows Service Trace view.
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            var sb = new StringBuilder();
            WriteHeader(sb);
            WriteEscaped(Message, sb);
            WriteFooter(sb);

            return sb.ToString();
        }

        private void WriteHeader(StringBuilder sb)
        {
            WriteStartHeader(sb);
            WriteEndHeader(sb);
        }

        private void WriteStartHeader(StringBuilder sb)
        {
            sb.Append("<E2ETraceEvent xmlns=\"http://schemas.microsoft.com/2004/06/E2ETraceEvent\"><System xmlns=\"http://schemas.microsoft.com/2004/06/windows/eventlog/system\">");
            sb.Append("<EventID>");
            sb.Append(((uint)EventId).ToString(CultureInfo.InvariantCulture));
            sb.Append("</EventID>");
            sb.Append("<Type>3</Type>");
            sb.Append("<SubType Name=\"");
            sb.Append(((object)EventType).ToString());
            sb.Append("\">0</SubType>");
            sb.Append("<Level>");
            int num = (int)EventType;
            if (num > (int)byte.MaxValue)
                num = (int)byte.MaxValue;
            if (num < 0)
                num = 0;
            sb.Append(num.ToString(CultureInfo.InvariantCulture));
            sb.Append("</Level>");
            sb.Append("<TimeCreated SystemTime=\"");
            sb.Append(DateTime.ToString("o", CultureInfo.InvariantCulture));
            sb.Append("\" />");
            sb.Append("<Source Name=\"");
            WriteEscaped(TraceSource, sb);
            sb.Append("\" />");
            sb.Append("<Correlation ActivityID=\"");
            sb.Append(ActivityId.ToString("B"));
        }

        private void WriteEndHeader(StringBuilder sb)
        {
            sb.Append("\" />");
            sb.Append("<Execution ProcessName=\"");
            sb.Append(ProcessName);
            sb.Append("\" ProcessID=\"");
            sb.Append(((uint)ProcessId).ToString(CultureInfo.InvariantCulture));
            sb.Append("\" ThreadID=\"");
            if (ThreadId != null)
                WriteEscaped(ThreadId.ToString(CultureInfo.InvariantCulture), sb);
            else
                WriteEscaped(Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture), sb);
            sb.Append("\" />");
            sb.Append("<Channel/>");
            sb.Append("<Computer>");
            sb.Append(MachineName);
            sb.Append("</Computer>");
            sb.Append("</System>");
            sb.Append("<ApplicationData>");
        }

        private void WriteFooter(StringBuilder sb)
        {
            sb.Append("<System.Diagnostics xmlns=\"http://schemas.microsoft.com/2004/08/System.Diagnostics\">");
            if (!String.IsNullOrEmpty(LogicalOperationStack))
            {
                sb.Append("<LogicalOperationStack>");
                String[] logicalOperationStack = LogicalOperationStack.Split('.');
                if (logicalOperationStack.Length > 0)
                {
                    foreach (var lop in logicalOperationStack)
                    {
                        sb.Append("<LogicalOperation>");
                        WriteEscaped(lop, sb);
                        sb.Append("</LogicalOperation>");
                    }
                }
                sb.Append("</LogicalOperationStack>");
            }
            sb.Append("<Timestamp>");
            sb.Append(Timestamp.ToString(CultureInfo.InvariantCulture));
            sb.Append("</Timestamp>");
            if (!String.IsNullOrEmpty(CallStack))
            {
                sb.Append("<Callstack>");
                WriteEscaped(CallStack, sb);
                sb.Append("</Callstack>");
            }
            sb.Append("</System.Diagnostics>");

            if (!String.IsNullOrEmpty(Activities))
            {
                sb.Append("<Activities>");
                WriteEscaped(Activities, sb);
                sb.Append("</Activities>");
            }
            sb.Append("</ApplicationData></E2ETraceEvent>");
        }

        private void WriteEscaped(string str, StringBuilder sb)
        {
            if (str == null)
                return;
            int startIndex = 0;
            for (int index = 0; index < str.Length; ++index)
            {
                switch (str[index])
                {
                    case '"':
                        sb.Append(str.Substring(startIndex, index - startIndex));
                        sb.Append("&quot;");
                        startIndex = index + 1;
                        break;
                    case '&':
                        sb.Append(str.Substring(startIndex, index - startIndex));
                        sb.Append("&amp;");
                        startIndex = index + 1;
                        break;
                    case '\'':
                        sb.Append(str.Substring(startIndex, index - startIndex));
                        sb.Append("&apos;");
                        startIndex = index + 1;
                        break;
                    case '<':
                        sb.Append(str.Substring(startIndex, index - startIndex));
                        sb.Append("&lt;");
                        startIndex = index + 1;
                        break;
                    case '>':
                        sb.Append(str.Substring(startIndex, index - startIndex));
                        sb.Append("&gt;");
                        startIndex = index + 1;
                        break;
                    case '\n':
                        sb.Append(str.Substring(startIndex, index - startIndex));
                        sb.Append("&#xA;");
                        startIndex = index + 1;
                        break;
                    case '\r':
                        sb.Append(str.Substring(startIndex, index - startIndex));
                        sb.Append("&#xD;");
                        startIndex = index + 1;
                        break;
                }
            }
            sb.Append(str.Substring(startIndex, str.Length - startIndex));
        }
        #endregion
    }
}
