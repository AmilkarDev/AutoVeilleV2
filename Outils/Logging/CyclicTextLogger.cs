using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Outils.Logging
{
    public class CyclicTextLogger : BaseListener
    {
        #region Private Properties/Fields

        /// <summary>
        /// Base configuration parameters.
        /// </summary>
        private CyclicTextLoggerCfg _config;

        /// <summary>
        /// The template to use when creating a new file cycle.
        /// </summary>
        private string _fileNameTemplate = null;

        /// <summary>
        /// When logging was initiated.
        /// </summary>
        private readonly DateTime _start = DateTime.Now;

        /// <summary>
        /// Existing files in the log.
        /// </summary>
        private readonly Queue<int> _filesInCycle = new Queue<int>();

        /// <summary>
        /// The file we are currently writing to.
        /// </summary>
        private StreamWriter _currentFile = null;

        /// <summary>
        /// Used to serialize requests to the logger.
        /// </summary>
        private readonly object _locker = new object();

        #endregion

        #region Constructors and Initialization

        public CyclicTextLogger()
        {
        }

        /// <summary>
        /// Configure the logger.  Since we cannot do it at construction time and there is no initialization means provided, we call
        /// this method before each I/O operation.  It will do anything only the first time through.
        /// </summary>
        protected bool Initialize()
        {
            if (_initialized || (_encounteredError && (DateTime.Now - _lastInitAttempt).TotalSeconds < 120))
                return _initialized;
            _lastInitAttempt = DateTime.Now;
            _encounteredError = false;

            try
            {
                //--- Extract attributes from the configuration.

                var directory = Attributes.ContainsKey("directory") ? Attributes["directory"] : "~/";
                if (directory[0] == '~')
                    directory = directory.Replace("~/", Utils.ApplicationPath);
                _config = new CyclicTextLoggerCfg
                {
                    Directory = directory,
                    BaseName =
                        Attributes.ContainsKey("baseName")
                            ? Attributes["baseName"]
                            : "$mLkbLog$n.log",
                    MaxSize =
                        Attributes.ContainsKey("maxSize")
                            ? int.Parse(Attributes["maxSize"])
                            : int.MaxValue,
                    Cycle = Attributes.ContainsKey("cycle") ? int.Parse(Attributes["cycle"]) : 1
                };
                if (!_config.Directory.EndsWith("\\"))
                    _config.Directory += "\\";

                //--- Build the file name template.

                var result = new StringBuilder(_config.BaseName);
                result.Replace("$m", _processName);
                result.Replace("$n", "N????");
                _fileNameTemplate = result.ToString();

                //--- Prepare the environment and the cycles.

                ExtractCurrentCycles();
                DropOldLogFiles();
                PrepareCurrentLogFile();

                _initialized = true;
            }
            catch (Exception ex)
            {
                _encounteredError = false;
                LogSystemError("Failed to initialize cyclic trace listener: {0}", ex.Message);
            }

            return _initialized;
        }

        /// <summary>
        /// Returns the list of our attributes.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// These attributes are included in the configuration declaration of the listener.
        /// </remarks>
        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "directory", "baseName", "maxSize", "cycle" };
        }

        /// <summary>
        /// Loads the list of the existing cycle files.
        /// </summary>
        private void ExtractCurrentCycles()
        {
            int cycleOffset = _fileNameTemplate.IndexOf("N????") + 1;
            if (!Directory.Exists(_config.Directory))
                Directory.CreateDirectory(_config.Directory);
            string[] files = Directory.GetFiles(_config.Directory, _fileNameTemplate);
            if (files != null)
            {
                Array.Sort(files);
                foreach (var file in files)
                    _filesInCycle.Enqueue(int.Parse(Path.GetFileName(file).Substring(cycleOffset, 4)));
            }
        }

        #endregion

        #region Public Interface

        /// <summary>
        /// We handle thread safety.
        /// </summary>
        public override bool IsThreadSafe
        {
            get { return true; }
        }

        /// <summary>
        /// We close the current file.  The logger remains usable.
        /// </summary>
        /// <remarks>
        /// The logger is designed to operate as standard trace listener.
        /// </remarks>
        public override void Close()
        {
            _initialized = false;
            lock (_locker)
                if (_currentFile != null)
                {
                    _currentFile.Close();
                    _currentFile = null;
                }
        }

        /// <summary>
        /// Fluses the content of the file.  This may be done manually or triggered automatically after each call by a configuration parm.
        /// </summary>
        public override void Flush()
        {
            lock (_locker)
                if (_currentFile != null)
                    _currentFile.Flush();
        }

        protected override void WriteEvent(TraceEventDescriptor eventDescr)
        {
            if (!Initialize())
                return;

            try
            {
                //--- Determine if the message must be emitted.

                if (string.IsNullOrEmpty(eventDescr.Message))
                    return;

                //--- Build the text of the message to send.
                var message = eventDescr.Message;
                if (!string.IsNullOrEmpty(eventDescr.CallStack))
                    message += "\r\n" + eventDescr.CallStack;
                if (!message.EndsWith("\n"))
                    message += "\r\n";

                //--- Split the message in its various lines.

                var lines = message.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var duration = eventDescr.DateTime - _start;
                var charDuration = String.Format("{0,2:d2}.{1,2:d2}:{2,2:d2}:{3,2:d2}.{4,3:d3}",
                                                    (int)Math.Floor(duration.TotalDays), duration.Hours, duration.Minutes,
                                                    duration.Seconds, duration.Milliseconds);
                var prefix = string.Format("{0}({1}) [{2,2:d2}] ({3}) -- ",
                                                eventDescr.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                charDuration,
                                                eventDescr.ThreadId,
                                                eventDescr.EventType);

                var noPrefix = new string(' ', prefix.Length);

                //---	Write out the message and flush immediately.

                lock (_locker)
                {
                    PrepareCurrentLogFile();
                    int lineNumber = 0;
                    foreach (var line in lines)
                        InternalWriteLine((lineNumber++ == 0 ? prefix : noPrefix) + line);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Close();
                }
                catch
                {
                    // do nothing.
                }
                LogSystemError("Failed to write trace event to cyclic log: {0}", ex.Message);
            }

        }

        protected void InternalWriteLine(string str)
        {
            lock (_locker)
            {
                PrepareCurrentLogFile();
                _currentFile.WriteLine(str);
            }
        }

        protected void InternalWrite(string str)
        {
            lock (_locker)
            {
                PrepareCurrentLogFile();
                _currentFile.Write(str);
            }
        }

        #endregion

        #region Private Area: Log files Management

        /// <summary>
        /// The list of files in the cycle telles us the number of files that currently exist.
        /// This number may exceed the maximum number of acceptable files in the cycle.  This
        /// method drops the extra files.
        /// </summary>
        private void DropOldLogFiles()
        {
            while (_filesInCycle.Count > _config.Cycle)
                File.Delete(GetFileName(_filesInCycle.Dequeue()));
        }

        /// <summary>
        /// We make sure the file we are about to use still exists and may be opened for writing.
        /// We also check its size to determine if we neew to switch to the next file in the cycle.
        /// </summary>
        private void PrepareCurrentLogFile()
        {
            //---	When there is no current file, we open the current one.

            if (_currentFile == null)
                OpenCurrentLogFile();

            //---	If the file size exceeds the allowed maximum, switch to next file in cycle.

            if (_currentFile.BaseStream.Length > _config.MaxSize)
            {
                CreateNewCycle();
                DropOldLogFiles();
                OpenCurrentLogFile();
            }
        }

        /// <summary>
        /// Adds the next log file in the cycle.
        /// </summary>
        private void CreateNewCycle()
        {
            int cycle = _filesInCycle.Count == 0 ? 1 : _filesInCycle.ElementAt(_filesInCycle.Count - 1) + 1;
            _filesInCycle.Enqueue(cycle);
        }

        /// <summary>
        /// Opens the last lof file in the list.  If a log file is already opened, we close it.
        /// </summary>
        private void OpenCurrentLogFile()
        {
            if (_currentFile != null)
                _currentFile.Close();
            if (_filesInCycle.Count == 0)
                CreateNewCycle();
            _currentFile = new StreamWriter(File.Open(GetCurrentFileName(), FileMode.Append));
        }

        /// <summary>
        /// Retruns the file name currently in use.
        /// </summary>
        /// <returns>The file name with the path prepended.</returns>
        private string GetCurrentFileName()
        {
            return GetFileName(_filesInCycle.ElementAt(_filesInCycle.Count - 1));
        }

        /// <summary>
        /// Build the file name for the specified cycle
        /// </summary>
        /// <param name="pCycle">The cycle for which we want the file name.</param>
        /// <returns>The file name with the path prepended.</returns>
        private string GetFileName(int pCycle)
        {
            return _config.Directory + _fileNameTemplate.Replace("N????", string.Format("N{0,4:d4}", pCycle));
        }

        #endregion
    }
}
