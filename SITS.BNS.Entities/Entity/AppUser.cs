using SITS.BNS.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Entities.Entity
{
    public class AppUser : CommonEntity
    {
        public virtual string UserEmail { get; set; }
        public virtual string Password { get; set; }
        public virtual string FullName { get; set; }

    }
}
