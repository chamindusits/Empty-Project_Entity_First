using Microsoft.Extensions.Logging;
using SITS.BNS.Common.Interfaces;
using SITS.BNS.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SITS.BNS.Common
{
    public class ApplicationLogger : IApplicationLogger
    {
        private readonly ILogger _logger;

        public ApplicationLogger(ILogger<ApplicationLogger> logger)
        {
            _logger = logger;
        }
        public async Task<object> Log(LogLevel level, LogFormat logFormat)
        {

            logFormat.TimeStamp = logFormat.TimeStamp == null ? DateTime.Now : logFormat.TimeStamp;

            switch (level)
            {
                case LogLevel.Information:
                    logFormat.Severity = LogLevelProperties.INFO;
                    logFormat.StatusCode = logFormat.StatusCode == 0 ? (int)HttpStatusCode.OK : logFormat.StatusCode;
                    _logger.LogInformation("Message : {Message}, IsJsonFormat : {IsJsonFormat} , UserId : {UserId},  WorkClass : {WorkClass}", JsonSerializer.Serialize(logFormat), true, logFormat.UserID, logFormat.AccessType);
                    break;
                case LogLevel.Error:
                    logFormat.Severity = LogLevelProperties.ERROR;
                    logFormat.StatusCode = logFormat.StatusCode == 0 ? (int)HttpStatusCode.InternalServerError : logFormat.StatusCode;
                    _logger.LogError("Message : {Message}, IsJsonFormat : {IsJsonFormat} , UserId : {UserId},  WorkClass : {WorkClass}", JsonSerializer.Serialize(logFormat), true, logFormat.UserID, logFormat.AccessType);
                    break;
                case LogLevel.Warning:
                    logFormat.StatusCode = logFormat.StatusCode == 0 ? (int)HttpStatusCode.OK : logFormat.StatusCode;
                    logFormat.Severity = LogLevelProperties.WARNING;
                    _logger.LogWarning("Message : {Message}, IsJsonFormat : {IsJsonFormat} , UserId : {UserId},  WorkClass : {WorkClass}", JsonSerializer.Serialize(logFormat), true, logFormat.UserID, logFormat.AccessType);
                    break;
                case LogLevel.Critical:
                    logFormat.Severity = LogLevelProperties.FATAL;
                    logFormat.StatusCode = logFormat.StatusCode == 0 ? (int)HttpStatusCode.InternalServerError : logFormat.StatusCode;
                    _logger.LogCritical("Message : {Message}, IsJsonFormat : {IsJsonFormat} , UserId : {UserId},  WorkClass : {WorkClass}", JsonSerializer.Serialize(logFormat), true, logFormat.UserID, logFormat.AccessType);
                    break;
                case LogLevel.Debug:
                    logFormat.Severity = LogLevelProperties.DEBUG;
                    logFormat.StatusCode = logFormat.StatusCode == 0 ? (int)HttpStatusCode.OK : logFormat.StatusCode;
                    _logger.LogDebug("Message : {Message}, IsJsonFormat : {IsJsonFormat} , UserId : {UserId}, WorkClass : {WorkClass}", JsonSerializer.Serialize(logFormat), true, logFormat.UserID, logFormat.AccessType);
                    break;

                case LogLevel.Trace:
                    logFormat.Severity = LogLevelProperties.TRACE;
                    logFormat.StatusCode = logFormat.StatusCode == 0 ? (int)HttpStatusCode.OK : logFormat.StatusCode;
                    _logger.LogTrace("Message : {Message}, IsJsonFormat : {IsJsonFormat} , UserId : {UserId},  WorkClass : {WorkClass}", JsonSerializer.Serialize(logFormat), true, logFormat.UserID, logFormat.AccessType);
                    break;
            }

            return default;
        }
    }
}
