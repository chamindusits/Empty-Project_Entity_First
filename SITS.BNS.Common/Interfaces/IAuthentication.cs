using SITS.BNS.Entities.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Common.Interfaces
{
    public interface IAuthentication
    {
        Task<Authentication> Authenticate(string userID, Claim[] claims);
        Task<Authentication> Authenticate(AuthRequest authRequest);
        Task<AuthResponse> APICall(AuthRequest authRequest);
        string GenerateRefreshToken();
    }
}
