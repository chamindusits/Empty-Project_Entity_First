using SITS.BNS.Entities.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Common.Interfaces
{
    public interface ITokenRefresher
    {
        Task<Authentication> Refresh(RefreshRequest refreshRequest);
    }
}
