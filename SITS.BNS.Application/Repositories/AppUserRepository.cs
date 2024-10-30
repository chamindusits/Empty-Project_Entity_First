using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SITS.BNS.Application.Repositories.Common;
using SITS.BNS.Application.Repositories.Interfaces;
using SITS.BNS.Common.Exceptions;
using SITS.BNS.Entities.AuthModels;
using SITS.BNS.Entities.Common;
using SITS.BNS.Entities.Entity;
using SITS.BNS.Infrastructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Application.Repositories
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private readonly AuthConfig authConfig;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AppUserRepository> logger;
        private readonly IOptions<AppSettings> _appSettings;
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;

        public AppUserRepository(ApplicationDbContext context, IOptions<AppSettings> appSettings) : base(context)
        {
            this._appSettings = appSettings;

        }

        public async Task<int> SaveAppUser(AppUser appUser)
        {
            this.Add(appUser);
            return await this.SaveChangesAsync();
        }

        public async Task<AuthResponse> Authenticate(AuthRequest authRequest)
        {
            //logger.LogInformation($"In Authenticate Request {authRequest.USER_ID}");

            var user = this.Find(x => x.IsActive && x.UserEmail.Equals(authRequest.USER_ID)).FirstOrDefault();

            // var user = await _appContext.AppUsers.FirstOrDefaultAsync(x => x.UserEmail.Equals(authRequest.USER_ID) && x.IsActive);

            if (user != null)
            {
                bool verified = BCrypt.Net.BCrypt.Verify(authRequest.USER_PASSWORD, user.Password);
                if (verified)
                {
                    var authres = new AuthResponse();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenKey = Encoding.ASCII.GetBytes(this._appSettings.Value.AuthConfig.Key);
                    var tokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name,user.FullName),
                    new Claim("UserId", user.ID.ToString()),
                    new Claim("Email", user.UserEmail),
                     new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(this._appSettings.Value.AuthConfig.ExpiresIn),
                        SigningCredentials = new
                        SigningCredentials(
                            new SymmetricSecurityKey(tokenKey),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    authres.Success = true;
                    authres.RefreshToken = String.Empty;
                    authres.JwtToken = tokenHandler.WriteToken(token);
                    authres.UserID = user.ID.ToString();
                    authres.Email = user.UserEmail.ToString();
                    authres.Name = user.FullName;
                    return authres;


                }
                else
                {
                    //logger.LogError($"In Authenticate Failed : Invalid Credentials {authRequest.USER_ID}");

                    throw new NotFoundException("Invalid Credentials");
                }
            }
            else
            {
                // logger.LogError($"In Authenticate Failed : User not found {authRequest.USER_ID}");
                throw new NotFoundException("User not found or Validate your email");
            }

        }


        public async Task<AppUser> GetAppUserById(int id)
        {
            var appuser = this.Find(x => x.IsActive && x.ID == id).FirstOrDefault();
            if (appuser == null)
            {
                throw new NotFoundException("User Not found");

            }
            else
            {
                return appuser;
            }
        }

    }
}
