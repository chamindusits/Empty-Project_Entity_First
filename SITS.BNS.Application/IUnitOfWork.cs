using SITS.BNS.Application.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Application
{
    public interface IUnitOfWork
    {
        IAppUserRepository AppUsers { get; }
        Task<int> SaveChangesAsync();
    }
}
