using Microsoft.Extensions.Logging;
using SITS.BNS.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Common.Interfaces
{
    public interface IApplicationLogger
    {
        /// <summary>
        /// Logging all the details using serilog
        /// </summary>
        /// <param name="level"></param>
        /// <param name="logFormat"></param>
        /// <returns>void</returns>
        public Task<object> Log(LogLevel level, LogFormat logFormat);
    }
}
