using SITS.BNS.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Infrastructure
{
    public static class ApplicationDbContextSeed
    {
        public static async Task<Task> SeedSampleDataAsync(ApplicationDbContext context)
        {

            if (!context.AppUsers.Any())
            {
                try
                {
                    var appuser = new Entities.Entity.AppUser { FullName = "SYS Admin", IsActive = true, Password = BCrypt.Net.BCrypt.HashPassword("bns@123"), UserEmail = "bns@bns.lk", CreatedBy = "SYS", CreatedByName = "SYS", CreatedDate = DateTime.Now.ToUniversalTime(), UpdatedBy = "SYS", UpdatedByName = "SYS", UpdatedDate = DateTime.Now.ToUniversalTime() };
                    context.AppUsers.Add(appuser);
                    await context.SaveChangesAsync();
                    var upadminapproles = new AppRole { IsActive = true, Name = "BNSAdmin", Description = "BNS Administrator" };
                    context.AppRoles.Add(upadminapproles);
                    await context.SaveChangesAsync();
                    context.AppUserRoles.Add(new AppUserRole { IsActive = true, appRoleId = upadminapproles.ID, appUserId = appuser.ID });

                    await context.SaveChangesAsync();

                }
                catch (Exception e)
                {

                    throw;
                }
            }

            if (!context.AppRoles.Any())
            {

            }
            return Task.CompletedTask;
        }
    }
}
