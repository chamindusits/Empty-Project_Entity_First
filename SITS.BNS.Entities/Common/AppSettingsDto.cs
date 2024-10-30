using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Entities.Common
{
    public class AppSettings
    {
        public AuthConfig AuthConfig { get; set; }
        public TimerJobConfig TimerJobConfig { get; set; }
        public OtherConfig OtherConfig { get; set; }

    }

    public class AuthConfig
    {
        public string Key { get; set; }
        public int ExpiresIn { get; set; }
    }

    public class OtherConfig
    {
        public string SubSite { get; set; }

    }

    public class TimerJobConfig
    {
        public string HostUrl { get; set; }
        public int OverdueDaysValidation { get; set; }
    }

}
