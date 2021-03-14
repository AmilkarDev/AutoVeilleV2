using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Outils.Logging
{
    [Serializable]
    class CyclicTextLoggerCfg
    {
        /// <summary>
        /// Name of directory into whch logging information will be saved.
        /// </summary>
        [XmlAttribute("directory")]
        public string Directory;

        /// <summary>
        /// Base name of the log file.
        /// </summary>
        [XmlAttribute("baseName")]
        public string BaseName;

        /// <summary>
        /// Number of files in the cycle.
        /// </summary>
        [XmlAttribute("cycle")]
        public int Cycle = -1;

        /// <summary>
        /// Maximum size, in bytes of a log file.
        /// </summary>
        [XmlAttribute("maxSize")]
        public int MaxSize = 0;
    }
}
