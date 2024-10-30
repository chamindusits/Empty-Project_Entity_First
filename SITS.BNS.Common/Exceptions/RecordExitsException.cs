using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Common.Exceptions
{
    public class RecordExitsException : Exception
    {

        public string ErrorMessage { get; }
        public RecordExitsException()
            : base()
        {
            ErrorMessage = string.Empty;
        }
        public RecordExitsException(string message)
            : base(message)
        {
            ErrorMessage = message;

        }
        public RecordExitsException(string message, Exception innerException)
           : base(message, innerException)
        {
            ErrorMessage = message;
        }
    }
}
