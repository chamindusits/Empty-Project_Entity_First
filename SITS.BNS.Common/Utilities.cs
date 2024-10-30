using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Common
{
    public static class Utilities
    {
        static ILoggerFactory _loggerFactory;


        public static void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }


        public static ILogger CreateLogger<T>()
        {
            //Usage: Utilities.CreateLogger<SomeClass>().LogError(LoggingEvents.SomeEventId, ex, "An error occurred because of xyz");

            if (_loggerFactory == null)
            {
                throw new InvalidOperationException($"{nameof(ILogger)} is not configured. {nameof(ConfigureLogger)} must be called before use");
                //_loggerFactory = new LoggerFactory().AddConsole().AddDebug();
            }

            return _loggerFactory.CreateLogger<T>();
        }


        public static void QuickLog(string text, string filename)
        {
            string dirPath = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using (StreamWriter writer = File.AppendText(filename))
            {
                writer.WriteLine($"{DateTime.Now} - {text}");
            }
        }



        public static string GetUserId(ClaimsPrincipal user)
        {
            //return user.FindFirst(JwtClaimTypes.Subject)?.Value?.Trim();
            return user.Identity.Name.Trim();

        }

        public static string GetBranchId(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;

            return identity.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value).FirstOrDefault();

        }

        public static string[] GetRoles(ClaimsPrincipal identity)
        {
            var x = (ClaimsIdentity)identity.Identity;

            return x.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();
        }
    }
}
