using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SITS.BNS.Entities.Common;
using SITS.BNS.Entities.Interfaces;
using SITS.BNS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Application
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(ApplicationDbContext context, IHostingEnvironment env, IHttpContextAccessor httpAccessor, IOptions<AppSettings> _appSettings, ICurrentUserService iCurrentUserService) : base(context, env, httpAccessor, _appSettings, iCurrentUserService)
        {
            context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst("ClaimConstants.Subject")?.Value?.Trim();
        }
    }
}
