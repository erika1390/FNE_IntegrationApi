using Integration.Application.Interfaces.Security;
using Integration.Core.Entities.Audit;
using Integration.Core.Entities.Parametric;
using Integration.Core.Entities.Security;
using Integration.Core.Interfaces.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Integration.Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<
        User, Role, int,
        UserClaim, UserRole,
        UserLogin, RoleClaim,
        UserToken>
    {
        private readonly ICurrentUserService _currentUserService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }
        private static readonly DateTime StaticCreatedAt = StaticCreatedAt;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Integration.Core.Entities.Security.Application> Applications { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public override DbSet<User> Users { get; set; }
        public override DbSet<UserRole> UserRoles { get; set; }
        public override DbSet<UserClaim> UserClaims { get; set; }
        public override DbSet<UserLogin> UserLogins { get; set; }
        public override DbSet<UserToken> UserTokens { get; set; }
        public override DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<IdentificationDocumentType> IdentificationDocumentType { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            builder.Entity<Integration.Core.Entities.Security.Application>().HasData(
                new Integration.Core.Entities.Security.Application
                {
                    Id = 1,
                    Code = "APP0000001",
                    Name = "Integrador",                    
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",                    
                    IsActive = true
                },
                new Integration.Core.Entities.Security.Application
                {
                    Id = 2,
                    Code = "APP0000002",
                    Name = "Saga 2.0",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Integration.Core.Entities.Security.Application
                {
                    Id = 3,
                    Code = "APP0000003",
                    Name = "Sicof Lite",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                }
            );
            builder.Entity<Module>().HasData(
                new Module
                {
                    Id = 1,
                    Code = "MOD0000001",
                    Name = "Configuración",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
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
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Permission
                {
                    Id = 2,
                    Code = "PER0000002",
                    Name = "Crear",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Permission
                {
                    Id = 3,
                    Code = "PER0000003",
                    Name = "Modificar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Permission
                {
                    Id = 4,
                    Code = "PER0000004",
                    Name = "Desactivar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Permission
                {
                    Id = 5,
                    Code = "PER0000005",
                    Name = "Cargar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Permission
                {
                    Id = 6,
                    Code = "PER0000006",
                    Name = "Descargar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
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
                    CreatedBy = "system",
                    CreatedAt = StaticCreatedAt,
                    UpdatedBy = "system",
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
            builder.Entity<IdentificationDocumentType>().HasData(
                new IdentificationDocumentType
                {
                    Id = 1,
                    Abbreviation = "CC",
                    Description = "Cedula Ciudadania",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new IdentificationDocumentType
                {
                    Id = 2,
                    Abbreviation = "CE",
                    Description = "Cedula Extrangeria",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new IdentificationDocumentType
                {
                    Id = 3,
                    Abbreviation = "PA",
                    Description = "Pasaporte",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new IdentificationDocumentType
                {
                    Id = 4,
                    Abbreviation = "PE",
                    Description = "Permiso Especial de permanencia",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new IdentificationDocumentType
                {
                    Id = 5,
                    Abbreviation = "PT",
                    Description = "Permiso por protección temporal",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new IdentificationDocumentType
                {
                    Id = 6,
                    Abbreviation = "NI",
                    Description = "Nit",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
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
                    entity.CreatedBy = _currentUserService.UserName;
                    GenerateEntityCode(entry);
                }
                else
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = _currentUserService.UserName;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        private void GenerateEntityCode(EntityEntry entry)
        {
            if (entry.Entity.GetType().GetProperty("Code") == null)
                return;
            var currentCode = entry.Property("Code").CurrentValue as string;
            if (!string.IsNullOrEmpty(currentCode))
                return;
            string prefix;
            string nextCode;
            if (entry.Entity is Integration.Core.Entities.Security.Application)
            {
                prefix = "APP";
                nextCode = GetNextCode<Integration.Core.Entities.Security.Application>(prefix);
            }
            else if (entry.Entity is Module)
            {
                prefix = "MOD";
                nextCode = GetNextCode<Module>(prefix);
            }
            else if (entry.Entity is User)
            {
                prefix = "USR";
                nextCode = GetNextCode<User>(prefix);
            }
            else if (entry.Entity is Permission)
            {
                prefix = "PER";
                nextCode = GetNextCode<Permission>(prefix);
            }
            else if (entry.Entity is Role)
            {
                prefix = "ROL";
                nextCode = GetNextCode<Role>(prefix);
            }
            else if (entry.Entity is Menu)
            {
                prefix = "MNU";
                nextCode = GetNextCode<Menu>(prefix);
            }
            else
            {
                return;
            }

            entry.Property("Code").CurrentValue = nextCode;
        }

        private string GetNextCode<T>(string prefix) where T : class
        {
            int lastNumber = 0;

            // Buscar el último número utilizado para este prefijo
            var propertyInfo = typeof(T).GetProperty("Code");
            if (propertyInfo != null)
            {
                var dbSet = this.Set<T>();
                var lastCode = dbSet.AsEnumerable()
                    .Where(e => propertyInfo.GetValue(e) != null && propertyInfo.GetValue(e).ToString().StartsWith(prefix))
                    .Select(e => propertyInfo.GetValue(e).ToString().Substring(prefix.Length))
                    .Select(c =>
                    {
                        if (int.TryParse(c, out int num))
                            return num;
                        return 0;
                    })
                    .DefaultIfEmpty(0)
                    .Max();

                lastNumber = lastCode;
            }

            // Incrementar para obtener el siguiente número
            int nextNumber = lastNumber + 1;

            // Formatear con 7 dígitos y el prefijo
            return $"{prefix}{nextNumber:D7}";
        }
    }
}