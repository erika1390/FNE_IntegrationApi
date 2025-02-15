using Integration.Core.Entities.Security;
using Integration.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Integration.Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<
        User, Role, int,
        UserClaim, UserRole,
        UserLogin, RoleClaim,
        UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleModule> RoleModules { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🔹 Definir el esquema "Security" para todas las entidades
            builder.Entity<User>().ToTable("Users", "Security");
            builder.Entity<Role>().ToTable("Roles", "Security");
            builder.Entity<UserRole>().ToTable("UserRoles", "Security");
            builder.Entity<RoleModule>().ToTable("RoleModules", "Security");
            builder.Entity<RolePermission>().ToTable("RolePermissions", "Security");
            builder.Entity<Permission>().ToTable("Permissions", "Security");
            builder.Entity<Application>().ToTable("Applications", "Security");
            builder.Entity<Module>().ToTable("Modules", "Security");
            builder.Entity<UserClaim>().ToTable("UserClaims", "Security");
            builder.Entity<UserLogin>().ToTable("UserLogins", "Security");
            builder.Entity<UserToken>().ToTable("UserTokens", "Security");
            builder.Entity<RoleClaim>().ToTable("RoleClaims", "Security");

            // 🔹 Definir claves primarias
            builder.Entity<RoleModule>().HasKey(rm => rm.RoleModuleId);
            builder.Entity<RolePermission>().HasKey(rp => rp.RolePermissionId);
            builder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

            // 🔹 Configuración de `UserRole`
            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Configuración de `RoleModule`
            builder.Entity<RoleModule>()
                .HasOne(rm => rm.Role)
                .WithMany(r => r.RoleModules)
                .HasForeignKey(rm => rm.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RoleModule>()
                .HasOne(rm => rm.Module)
                .WithMany(m => m.RoleModules)
                .HasForeignKey(rm => rm.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Configuración de relaciones
            builder.Entity<Permission>()
                .HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .HasPrincipalKey(p => p.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);  // 🔹 Forzar la relación exacta

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.RoleModule)
                .WithMany(rm => rm.RolePermissions)
                .HasForeignKey(rp => rp.RoleModuleId)
                .OnDelete(DeleteBehavior.Restrict);
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