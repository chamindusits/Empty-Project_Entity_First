using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using SITS.BNS.Entities.Entity;
using SITS.BNS.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private IDbContextTransaction _currentTransaction;
        protected readonly IConfiguration Configuration;
        public ApplicationDbContext(DbContextOptions options, ICurrentUserService currentUserService, IConfiguration configuration) : base(options)
        {
            _currentUserService = currentUserService;
            Configuration = configuration;
        }
        public string CurrentUserId { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>().Property(p => p.ID).ValueGeneratedOnAdd();
            builder.Entity<AppUser>().ToTable($"UIA_{nameof(this.AppUsers)}");

            builder.Entity<AppRole>().Property(p => p.ID).ValueGeneratedOnAdd();
            builder.Entity<AppRole>().ToTable($"UIA_{nameof(this.AppRoles)}");

            builder.Entity<AppUserRole>().Property(p => p.ID).ValueGeneratedOnAdd();
            builder.Entity<AppUserRole>().ToTable($"UIA_{nameof(this.AppUserRoles)}");



        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }


        public override int SaveChanges()
        {
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entry in ChangeTracker.Entries<CommonEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedByName = _currentUserService.Name;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.UpdatedDate = DateTime.Now;
                        entry.Entity.UpdatedByName = _currentUserService.Name;
                        break;
                }
            }



            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                await RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
