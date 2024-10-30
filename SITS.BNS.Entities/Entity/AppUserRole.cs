using SITS.BNS.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Entities.Entity
{
    public class AppUserRole : CommonEntity
    {
        public virtual int appUserId { get; set; }
        public virtual AppUser appUser { get; set; }
        public virtual int appRoleId { get; set; }
        public virtual AppRole appRole { get; set; }

    }
}
