using Integration.Core.Entities.Audit;
using Integration.Core.Entities.Security;
using Integration.Core.Interfaces.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Integration.Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<
        User, Role, int,
        UserClaim, UserRole,
        UserLogin, RoleClaim,
        UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Module> Modules { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleModule> RoleModules { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public override DbSet<User> Users { get; set; }
        public override DbSet<UserRole> UserRoles { get; set; }
        public override DbSet<UserClaim> UserClaims { get; set; }
        public override DbSet<UserLogin> UserLogins { get; set; }
        public override DbSet<UserToken> UserTokens { get; set; }
        public override DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        /// <summary>
        /// Sobrescribir SaveChangesAsync para aplicar auditoría automáticamente
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreatedBy = "System"; // Reemplázalo con el usuario autenticado
                }
                else
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = "System"; // Reemplázalo con el usuario autenticado
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}