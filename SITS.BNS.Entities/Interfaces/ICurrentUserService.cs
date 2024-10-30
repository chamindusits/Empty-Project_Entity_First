using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Entities.Interfaces
{
    public interface ICurrentUserService
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AccessType { get; set; }
    }
}
