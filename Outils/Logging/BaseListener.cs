using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Threading;
using System.Globalization;

namespace Outils.Logging
{
    public abstract class BaseListener : TraceListener
    {/// <summary>
        /// cached value, containing the current process name
        /// </summary>
        protected string _processName;

        /// <summary>
        /// ID of the current process.
        /// </summary>
        protected int _processId;

        /// <summary>
        /// Indicates the the listener encountered an error that prevented it from 
        /// completing it's task. 
        /// </summary>
        protected bool _encounteredError;

        /// <summary>
        /// Indicated the timestamp of the last time an initialization was attempted.
        /// </summary>
        protected DateTime _lastInitAttempt = DateTime.MinValue;

        /// <summary>
        /// Indicates that we are initialized.
        /// </summary>
        protected bool _initialized;

        /// <summary>
        /// This is the obly abstract method that needs to be implemented by specializing classes.
        /// The implementation is responsible for the final treatment of the event.
        /// </summary>
        /// <param name="eventDescr">complete description of the event</param>
        protected abstract void WriteEvent(TraceEventDescriptor eventDescr);

        #region Public Interface

        /// <summary>
        /// We handle thread safety.
        /// </summary>
        public override bool IsThreadSafe
        {
            get { return true; }
        }

        /// <summary>
        /// Will call WriteEvent once for every acitvity currently in this threads acivity list (see ActivitySet)
        /// </summary>
        /// <param name="pEventCache"></param>
        /// <param name="pSource"></param>
        /// <param name="pEventType"></param>
        /// <param name="pId"></param>
        /// <param name="pMsgKey"></param>
        public override void TraceEvent(TraceEventCache pEventCache, String pSource, TraceEventType pEventType, int pId, String pMsgKey)
        {
            var activities = ActivitySet.ToString();
            ActivitySet.Activities.ForEach(x =>
            {
                var ed = BuildEventObject(pEventCache, pSource, pEventType, pId, pMsgKey, null);
                ed.ActivityId = x;
                ed.Activities = activities;
                WriteEvent(ed);
            });
        }

        /// <summary>
        /// Will call WriteEvent once for every acitvity currently in this threads acivity list (see ActivitySet)
        /// </summary>
        /// <param name="pEventCache"></param>
        /// <param name="pSource"></param>
        /// <param name="pEventType"></param>
        /// <param name="pId"></param>
        /// <param name="pMsgKey"></param>
        /// <param name="pArgs">note que ne peut contenir que un seul EventObj, tout autre est ignore.</param>
        public override void TraceEvent(TraceEventCache pEventCache, String pSource, TraceEventType pEventType, int pId, String pMsgKey, params Object[] pArgs)
        {
            var activities = ActivitySet.ToString();
            ActivitySet.Activities.ForEach(x =>
            {
                var ed = BuildEventObject(pEventCache, pSource, pEventType, pId, pMsgKey, pArgs);
                ed.ActivityId = x;
                ed.Activities = activities;
                WriteEvent(ed);
            });
        }

        /// <summary>
        /// Will call WriteEvent once for every acitvity currently in this threads acivity list (see ActivitySet)
        /// </summary>
        /// <param name="pMsgKey">The key to the messages in the resource file.</param>
        public override void WriteLine(String pMsgKey)
        {
            var activities = ActivitySet.ToString();
            ActivitySet.Activities.ForEach(x =>
            {
                var ed = new TraceEventDescriptor
                {
                    Message = pMsgKey,
                    EventType = TraceEventType.Information,
                    ThreadId =
                        Thread.CurrentThread.ManagedThreadId.ToString(
                            CultureInfo.InvariantCulture),
                    Timestamp = Stopwatch.GetTimestamp(),
                    DateTime = DateTime.Now.ToUniversalTime(),
                    ProcessName = Utils.ApplicationName,
                    ProcessId = _processId,
                    MachineName = Environment.MachineName,
                    TraceSource = "Trace",
                    Activities = activities
                };
                ed.ActivityId = x;
                WriteEvent(ed);
            });
        }

        /// <summary>
        /// Will call WriteEvent once for every acitvity currently in this threads acivity list (see ActivitySet)
        /// </summary>
        /// <param name="pMsgKey">The key to the messages in the resource file.</param>
        public override void Write(String pMsgKey)
        {
            WriteLine(pMsgKey);
        }

        /// <summary>
        /// Writes trace information including an error message and a detailed error message to the file or stream.
        /// Will call WriteEvent once for every acitvity currently in this threads acivity list (see ActivitySet)
        /// </summary>
        /// <param name="message">The error message to write.</param><param name="detailMessage">The detailed error message to append to the error message.</param>
        public override void Fail(string message, string detailMessage)
        {
            var stringBuilder = new StringBuilder(message);
            if (!String.IsNullOrEmpty(detailMessage))
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(detailMessage);
            }

            var activities = ActivitySet.ToString();

            ActivitySet.Activities.ForEach(x =>
            {
                var ed = new TraceEventDescriptor
                {
                    Message = stringBuilder.ToString(),
                    EventType = TraceEventType.Error,
                    ThreadId =
                        Thread.CurrentThread.ManagedThreadId.ToString(
                            CultureInfo.InvariantCulture),
                    Timestamp = Stopwatch.GetTimestamp(),
                    DateTime = DateTime.Now.ToUniversalTime(),
                    ProcessName = Utils.ApplicationName,
                    ProcessId = _processId,
                    MachineName = Environment.MachineName,
                    TraceSource = "Trace",
                    Activities = activities
                };
                ed.ActivityId = x;
                WriteEvent(ed);
            });
        }

        #endregion

        /// <summary>
        /// Method that constructs a TraceEventDescriptor from the parameters provided.
        /// </summary>
        /// <param name="pEventCache"></param>
        /// <param name="pSource"></param>
        /// <param name="pEventType"></param>
        /// <param name="pId"></param>
        /// <param name="pMsgKey"></param>
        /// <param name="pArgs"> </param>
        /// <returns></returns>
        private TraceEventDescriptor BuildEventObject(TraceEventCache pEventCache, String pSource, TraceEventType pEventType, int pId, String pMsgKey, Object[] pArgs)
        {
            var evt = new TraceEventDescriptor
            {
                EventId = pId,
                EventType = (TraceEventType)((int)pEventType & 0x1F),
                DateTime = pEventCache.DateTime.ToUniversalTime(),
                Timestamp = pEventCache.Timestamp,
                TraceSource = pSource,
                Message = (pArgs != null) ? String.Format(pMsgKey, pArgs) : pMsgKey,
                MachineName = Environment.MachineName,
                ProcessName = _processName,
            };

            if (Trace.CorrelationManager.LogicalOperationStack.Count > 0)
                evt.ActivityId = Trace.CorrelationManager.ActivityId;

            if ((TraceOutputOptions & TraceOptions.Callstack) == TraceOptions.Callstack || evt.EventType <= TraceEventType.Error)
                evt.CallStack = pEventCache.Callstack;
            else
                evt.CallStack = null;

            if ((TraceOutputOptions & TraceOptions.LogicalOperationStack) == TraceOptions.LogicalOperationStack)
            {
                var sb = new StringBuilder();

                foreach (Object x in pEventCache.LogicalOperationStack)
                {
                    if (sb.Length > 0)
                        sb.Insert(0, '.');
                    sb.Insert(0, x);
                }

                evt.LogicalOperationStack = sb.ToString();
            }
            else
                evt.LogicalOperationStack = null;

            if ((TraceOutputOptions & TraceOptions.ProcessId) == TraceOptions.ProcessId)
                evt.ProcessId = pEventCache.ProcessId;

            if ((TraceOutputOptions & TraceOptions.ThreadId) == TraceOptions.ThreadId)
                evt.ThreadId = pEventCache.ThreadId;

            return evt;
        }

        /// <summary>
        /// helper checking if given trace option is present in the trace out options
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        protected bool IsEnabled(TraceOptions opts)
        {
            return (opts & TraceOutputOptions) != TraceOptions.None;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        protected BaseListener()
        {
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

            Process currentProcess = Process.GetCurrentProcess();
            _processName = Utils.ApplicationName;
            _processId = currentProcess.Id;
        }

        /// <summary>
        /// When the logging system itself generates an error, this is called.
        ///     - Log the event in the windows event log
        ///     - dectivate the logging by this instance for 2 minutes.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void LogSystemError(String message, params Object[] args)
        {
            try
            {
                _encounteredError = true;
                var source = _processName;

                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, "Application");

                if (args != null && args.Length > 0)
                    message = String.Format(message, args);

                EventLog.WriteEntry(source, message, EventLogEntryType.Error);
            }
            catch
            {
                // oh well.. we tried...
            }
        }
    }
}
