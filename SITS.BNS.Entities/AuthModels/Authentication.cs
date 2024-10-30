using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Entities.AuthModels
{
    public class Authentication
    {
        public bool Success { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserID { get; set; }
        public string AccessType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
