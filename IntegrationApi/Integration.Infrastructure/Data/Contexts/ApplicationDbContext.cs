using Integration.Core.Entities.Audit;
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
        private static readonly DateTime StaticCreatedAt = StaticCreatedAt;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Module> Modules { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleModulePermissions> RoleModulePermissions { get; set; }
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
            builder.Entity<Application>().HasData(
                new Application
                {
                    Id = 1,
                    Code = "APP0000001",
                    Name = "Integrador",                    
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",                    
                    IsActive = true
                },
                new Application
                {
                    Id = 2,
                    Code = "APP0000002",
                    Name = "Saga 2.0",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Application
                {
                    Id = 3,
                    Code = "APP0000003",
                    Name = "Sicof Lite",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                }
            );
            builder.Entity<Module>().HasData(
                new Module
                {
                    Id = 1,
                    Code = "MOD0000001",
                    Name = "Gestión de Aplicaciones",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Module
                {
                    Id = 2,
                    Code = "MOD0000002",
                    Name = "Gestión de Módulos",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Module
                {
                    Id = 3,
                    Code = "MOD0000003",
                    Name = "Gestión de Permisos",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Module
                {
                    Id = 4,
                    Code = "MOD0000004",
                    Name = "Gestión de Roles",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Module
                {
                    Id = 5,
                    Code = "MOD0000005",
                    Name = "Gestión de Usuarios",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Module
                {
                    Id = 6,
                    Code = "MOD0000006",
                    Name = "Asignación de Permisos por Rol",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Module
                {
                    Id = 7,
                    Code = "MOD0000007",
                    Name = "Asignación de Roles por Usuario",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                }
            );
            builder.Entity<Permission>().HasData(
                new Permission
                {
                    Id = 1,
                    Code = "PER0000001",
                    Name = "Consultar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Permission
                {
                    Id = 2,
                    Code = "PER0000002",
                    Name = "Crear",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Permission
                {
                    Id = 3,
                    Code = "PER0000003",
                    Name = "Modificar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Permission
                {
                    Id = 4,
                    Code = "PER0000004",
                    Name = "Desactivar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Permission
                {
                    Id = 5,
                    Code = "PER0000005",
                    Name = "Cargar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                },
                new Permission
                {
                    Id = 6,
                    Code = "PER0000006",
                    Name = "Descargar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "epulido",
                    UpdatedBy = "epulido",
                    IsActive = true
                }
            );
            builder.Entity<User>().HasData(
                new User { 
                    Id = 1, 
                    Code = "USR0000001", 
                    FirstName = "Erika", 
                    LastName = "Pulido", 
                    DateOfBirth = new DateTime(1990, 12, 13, 0, 0, 0),
                    CreatedBy = "epulido",
                    CreatedAt = StaticCreatedAt,
                    UpdatedBy = "epulido",
                    UpdatedAt = StaticCreatedAt,
                    IsActive = true,
                    UserName = "epulido",
                    NormalizedEmail = "EPULIDO",
                    Email = "epulido@minsalud.gov.co",
                    NormalizedUserName = "EPULIDO@MINSALUD.GOV.CO",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==",
                    SecurityStamp = "2756991d-795c-4132-8848-34d79e60b300",
                    ConcurrencyStamp = "b69f36df-8915-4287-949e-80c1f0d99cf8",
                    PhoneNumber = "3157234493",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            );
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
                    entity.CreatedBy = "epulido"; // Reemplázalo con el usuario autenticado
                }
                else
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = "epulido"; // Reemplázalo con el usuario autenticado
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}