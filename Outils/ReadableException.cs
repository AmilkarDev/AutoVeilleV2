using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outils
{

    public class ReadableException : Exception
    {
        public ReadableException(string message)
            : base(message)
        {
        }

        public ReadableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public override string Message
        {
            get
            {
                var msg = base.Message;
                if (InnerException != null && InnerException is ReadableException)
                    msg += "\n -> " + InnerException.Message;
                return msg;
            }
        }
    }
}
