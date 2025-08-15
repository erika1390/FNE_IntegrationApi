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
        public static readonly DateTime StaticCreatedAt = new DateTime(2025, 7, 28, 0, 0, 0, DateTimeKind.Utc);

        // Constantes estáticas para ConcurrencyStamp
        private static readonly string ROLE_CONCURRENCY_STAMP_1 = "9d2736e7-7fef-4779-a2cc-474789c810fe";
        private static readonly string ROLE_CONCURRENCY_STAMP_2 = "358ddaa5-d5af-4e94-82aa-1dea712b41ff";
        private static readonly string ROLE_CONCURRENCY_STAMP_3 = "d837f09f-22a7-44a9-9ad3-5358fc25a463";
        private static readonly string ROLE_CONCURRENCY_STAMP_4 = "3c2fac15-ca08-4b21-9175-fee779734fa8";
        private static readonly string ROLE_CONCURRENCY_STAMP_5 = "6f5e165e-d753-4847-84b1-913142273d00";
        private static readonly string ROLE_CONCURRENCY_STAMP_6 = "b69f36df-8915-4287-949e-80c1f0d99cf8";
        private static readonly string ROLE_CONCURRENCY_STAMP_7 = "7f2a6c54-2d1e-4b37-9a5c-8f0e3b6a21d4";

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
        public DbSet<Department> Departments { get; set; }
        public DbSet<City> Cities { get; set; }

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
                },
                new Module
                {
                    Id = 2,
                    Code = "MOD0000002",
                    Name = "Administración",
                    ApplicationId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Module
                {
                    Id = 3,
                    Code = "MOD0000003",
                    Name = "Principal",
                    ApplicationId = 3,
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
                new User
                {
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
                },
                new User
                {
                    Id = 2,
                    Code = "USR0000002",
                    FirstName = "Sandra",
                    LastName = "Medina",
                    DateOfBirth = new DateTime(1990, 12, 13, 0, 0, 0),
                    CreatedBy = "system",
                    CreatedAt = StaticCreatedAt,
                    UpdatedBy = "system",
                    UpdatedAt = StaticCreatedAt,
                    IsActive = true,
                    UserName = "sjmedina",
                    NormalizedEmail = "SJMEDINA",
                    Email = "sjmedina@Minsalud.gov.co",
                    NormalizedUserName = "SJMEDINA@MINSALUD.GOV.CO",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==",
                    SecurityStamp = "2756991d-795c-4132-8848-34d79e60b300",
                    ConcurrencyStamp = "b69f36df-8915-4287-949e-80c1f0d99cf8",
                    PhoneNumber = "3157234494",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new User
                {
                    Id = 3,
                    Code = "USR0000003",
                    FirstName = "Estefania",
                    LastName = "Giraldo Chica",
                    DateOfBirth = new DateTime(1990, 12, 13, 0, 0, 0),
                    CreatedBy = "system",
                    CreatedAt = StaticCreatedAt,
                    UpdatedBy = "system",
                    UpdatedAt = StaticCreatedAt,
                    IsActive = true,
                    UserName = "egiraldo",
                    NormalizedEmail = "EGIRALDO",
                    Email = "egiraldo@Minsalud.gov.co",
                    NormalizedUserName = "EGIRALDO@MINSALUD.GOV.CO",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==",
                    SecurityStamp = "2756991d-795c-4132-8848-34d79e60b300",
                    ConcurrencyStamp = "b69f36df-8915-4287-949e-80c1f0d99cf8",
                    PhoneNumber = "3157234495",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new User
                {
                    Id = 4,
                    Code = "USR0000004",
                    FirstName = "Julian",
                    LastName = "Cuervo Bustamante",
                    DateOfBirth = new DateTime(1990, 12, 13, 0, 0, 0),
                    CreatedBy = "system",
                    CreatedAt = StaticCreatedAt,
                    UpdatedBy = "system",
                    UpdatedAt = StaticCreatedAt,
                    IsActive = true,
                    UserName = "jcuervo",
                    NormalizedEmail = "JCUERVO",
                    Email = "jcuervo@Minsalud.gov.co",
                    NormalizedUserName = "JCUERVO@MINSALUD.GOV.CO",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==",
                    SecurityStamp = "2756991d-795c-4132-8848-34d79e60b300",
                    ConcurrencyStamp = "b69f36df-8915-4287-949e-80c1f0d99cf8",
                    PhoneNumber = "3157234496",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new User
                {
                    Id = 5,
                    Code = "USR0000005",
                    FirstName = "William",
                    LastName = "Molina Morales",
                    DateOfBirth = new DateTime(1990, 12, 13, 0, 0, 0),
                    CreatedBy = "system",
                    CreatedAt = StaticCreatedAt,
                    UpdatedBy = "system",
                    UpdatedAt = StaticCreatedAt,
                    IsActive = true,
                    UserName = "wmolina",
                    NormalizedEmail = "WMOLINA",
                    Email = "wmolina@Minsalud.gov.co",
                    NormalizedUserName = "WMOLINA@MINSALUD.GOV.CO",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==",
                    SecurityStamp = "2756991d-795c-4132-8848-34d79e60b300",
                    ConcurrencyStamp = "b69f36df-8915-4287-949e-80c1f0d99cf8",
                    PhoneNumber = "3157234497",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new User
                {
                    Id = 6,
                    Code = "USR0000006",
                    FirstName = "Juan Felipe",
                    LastName = "Valencia Renteria",
                    DateOfBirth = new DateTime(1990, 12, 13, 0, 0, 0),
                    CreatedBy = "system",
                    CreatedAt = StaticCreatedAt,
                    UpdatedBy = "system",
                    UpdatedAt = StaticCreatedAt,
                    IsActive = true,
                    UserName = "jvalenciar",
                    NormalizedEmail = "JVALENCIAR",
                    Email = "jvalenciar@Minsalud.gov.co",
                    NormalizedUserName = "JVALENCIAR@MINSALUD.GOV.CO",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==",
                    SecurityStamp = "2756991d-795c-4132-8848-34d79e60b300",
                    ConcurrencyStamp = "b69f36df-8915-4287-949e-80c1f0d99cf8",
                    PhoneNumber = "3157234498",
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
            builder.Entity<Department>().HasData(
                new Department
                {
                    Id = 1,
                    CodeDane = "91",
                    Name = "Amazonas",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 2,
                    CodeDane = "05",
                    Name = "Antioquia",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 3,
                    CodeDane = "81",
                    Name = "Arauca",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 4,
                    CodeDane = "08",
                    Name = "Atlántico",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 5,
                    CodeDane = "11",
                    Name = "Bogotá, D.C.",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 6,
                    CodeDane = "13",
                    Name = "Bolívar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 7,
                    CodeDane = "15",
                    Name = "Boyacá",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 8,
                    CodeDane = "17",
                    Name = "Caldas",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 9,
                    CodeDane = "18",
                    Name = "Caquetá",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 10,
                    CodeDane = "85",
                    Name = "Casanare",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 11,
                    CodeDane = "19",
                    Name = "Cauca",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 12,
                    CodeDane = "20",
                    Name = "Cesar",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 13,
                    CodeDane = "27",
                    Name = "Chocó",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 14,
                    CodeDane = "23",
                    Name = "Córdoba",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 15,
                    CodeDane = "25",
                    Name = "Cundinamarca",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 16,
                    CodeDane = "94",
                    Name = "Guainía",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 17,
                    CodeDane = "95",
                    Name = "Guaviare",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 18,
                    CodeDane = "41",
                    Name = "Huila",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 19,
                    CodeDane = "44",
                    Name = "La Guajira",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 20,
                    CodeDane = "47",
                    Name = "Magdalena",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 21,
                    CodeDane = "50",
                    Name = "Meta",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 22,
                    CodeDane = "52",
                    Name = "Nariño",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 23,
                    CodeDane = "54",
                    Name = "Norte de Santander",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 24,
                    CodeDane = "86",
                    Name = "Putumayo",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 25,
                    CodeDane = "63",
                    Name = "Quindío",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 26,
                    CodeDane = "66",
                    Name = "Risaralda",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 27,
                    CodeDane = "88",
                    Name = "San Andrés, Providencia y Santa Catalina",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 28,
                    CodeDane = "68",
                    Name = "Santander",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 29,
                    CodeDane = "70",
                    Name = "Sucre",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 30,
                    CodeDane = "73",
                    Name = "Tolima",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 31,
                    CodeDane = "76",
                    Name = "Valle del Cauca",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 32,
                    CodeDane = "97",
                    Name = "Vaupés",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Department
                {
                    Id = 33,
                    CodeDane = "99",
                    Name = "Vichada",
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                }
            );

            builder.Entity<Menu>().HasData(
                new Menu
                {
                    Id = 1,
                    ParentMenuId = null,
                    ModuleId = 1,
                    Code = "MNU0000001",
                    Name = "Administración",
                    Route = "/administracion",
                    Icon = "cog-outline",
                    Order = 0,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 2,
                    ParentMenuId = 1,
                    ModuleId = 1,
                    Code = "MNU0000002",
                    Name = "Gestionar Aplicación",
                    Route = "/administracion/gestionar-aplicacion",
                    Icon = "grid-outline",
                    Order = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 3,
                    ParentMenuId = 1,
                    ModuleId = 1,
                    Code = "MNU0000003",
                    Name = "Gestionar Módulo",
                    Route = "/administracion/gestionar-modulo",
                    Icon = "layers-outline",
                    Order = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 4,
                    ParentMenuId = 1,
                    ModuleId = 1,
                    Code = "MNU0000004",
                    Name = "Gestionar Menú",
                    Route = "/administracion/gestionar-menu",
                    Icon = "menu-outline",
                    Order = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 5,
                    ParentMenuId = 1,
                    ModuleId = 1,
                    Code = "MNU0000005",
                    Name = "Gestionar Permisos",
                    Route = "/administracion/gestionar-permisos",
                    Icon = "lock-closed-outline",
                    Order = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 6,
                    ParentMenuId = 1,
                    ModuleId = 1,
                    Code = "MNU0000006",
                    Name = "Gestionar Usuarios",
                    Route = "/administracion/gestionar-usuarios",
                    Icon = "people-outline",
                    Order = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 7,
                    ParentMenuId = 1,
                    ModuleId = 1,
                    Code = "MNU0000007",
                    Name = "Gestionar Roles",
                    Route = "/administracion/gestionar-roles",
                    Icon = "extension-puzzle-outline",
                    Order = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 8,
                    ParentMenuId = 1,
                    ModuleId = 1,
                    Code = "MNU0000008",
                    Name = "Asignar Role a Usuario",
                    Route = "/administracion/asignar-roles",
                    Icon = "person-outline",
                    Order = 7,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 9,
                    ParentMenuId = 1,
                    ModuleId = 1,
                    Code = "MNU0000009",
                    Name = "Configuración de Permisos",
                    Route = "/administracion/configuracion-permisos",
                    Icon = "key-outline",
                    Order = 8,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 10,
                    ParentMenuId = null,
                    ModuleId = 2,
                    Code = "MNU0000010",
                    Name = "Administración",
                    Route = "/administracion",
                    Icon = "cog-outline",
                    Order = 0,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 11,
                    ParentMenuId = 10,
                    ModuleId = 2,
                    Code = "MNU0000011",
                    Name = "Gestionar Usuarios",
                    Route = "/administracion/gestionar-usuarios",
                    Icon = "people-outline",
                    Order = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 12,
                    ParentMenuId = 10,
                    ModuleId = 2,
                    Code = "MNU0000012",
                    Name = "Gestionar Roles",
                    Route = "/administracion/gestionar-roles",
                    Icon = "extension-puzzle-outline",
                    Order = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 13,
                    ParentMenuId = 10,
                    ModuleId = 2,
                    Code = "MNU0000013",
                    Name = "Asignar Role a Usuario",
                    Route = "/administracion/asignar-roles",
                    Icon = "person-outline",
                    Order = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 14,
                    ParentMenuId = 10,
                    ModuleId = 2,
                    Code = "MNU0000014",
                    Name = "Configuración de Permisos",
                    Route = "/administracion/configuracion-permisos",
                    Icon = "key-outline",
                    Order = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 15,
                    ParentMenuId = null,
                    ModuleId = 3,
                    Code = "MNU0000015",
                    Name = "Principal",
                    Route = "/interface",
                    Icon = "description",
                    Order = 0,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 16,
                    ParentMenuId = 15,
                    ModuleId = 3,
                    Code = "MNU0000016",
                    Name = "Panel de control",
                    Route = "/interface/inicio",
                    Icon = "description",
                    Order = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 17,
                    ParentMenuId = 15,
                    ModuleId = 3,
                    Code = "MNU0000017",
                    Name = "Tramite",
                    Route = "/interface/tramites",
                    Icon = "description",
                    Order = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 18,
                    ParentMenuId = 15,
                    ModuleId = 3,
                    Code = "MNU0000018",
                    Name = "Inscripciones",
                    Route = "/interface/inscripciones",
                    Icon = "description",
                    Order = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Menu
                {
                    Id = 19,
                    ParentMenuId = 15,
                    ModuleId = 3,
                    Code = "MNU0000019",
                    Name = "Novedades",
                    Route = "/interface/novedades",
                    Icon = "description",
                    Order = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                }
            );
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Code = "ROL0000001",
                    Name = "Administrador Integrador",
                    ConcurrencyStamp = ROLE_CONCURRENCY_STAMP_1,
                    NormalizedName = "ADMINISTRADOR INTEGRADOR",
                    ApplicationId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Role
                {
                    Id = 2,
                    Code = "ROL0000002",
                    Name = "Administrador Saga 2.0",
                    ConcurrencyStamp = ROLE_CONCURRENCY_STAMP_2,
                    NormalizedName = "ADMINISTRADOR SAGA 2.0",
                    ApplicationId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Role
                {
                    Id = 3,
                    Code = "ROL0000003",
                    Name = "Administrador Sicof Lite",
                    ConcurrencyStamp = ROLE_CONCURRENCY_STAMP_3,
                    NormalizedName = "ADMINISTRADOR SICOF LITE",
                    ApplicationId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Role
                {
                    Id = 4,
                    Code = "ROL0000004",
                    Name = "Asignador",
                    ConcurrencyStamp = ROLE_CONCURRENCY_STAMP_4,
                    NormalizedName = "ASIGNADOR",
                    ApplicationId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Role
                {
                    Id = 5,
                    Code = "ROL0000005",
                    Name = "Gestor",
                    ConcurrencyStamp = ROLE_CONCURRENCY_STAMP_5,
                    NormalizedName = "GESTOR",
                    ApplicationId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Role
                {
                    Id = 6,
                    Code = "ROL0000006",
                    Name = "Consultor",
                    ConcurrencyStamp = ROLE_CONCURRENCY_STAMP_6,
                    NormalizedName = "CONSULTOR",
                    ApplicationId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new Role
                {
                    Id = 7,
                    Code = "ROL0000007",
                    Name = "QuienRevisa",
                    ConcurrencyStamp = ROLE_CONCURRENCY_STAMP_7,
                    NormalizedName = "QUIENREVISA",
                    ApplicationId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                }
            );

            builder.Entity<RoleMenuPermission>().HasData(
                new RoleMenuPermission
                {
                    Id = 1,
                    RoleId = 1,
                    MenuId = 2,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 2,
                    RoleId = 1,
                    MenuId = 2,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 3,
                    RoleId = 1,
                    MenuId = 2,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 4,
                    RoleId = 1,
                    MenuId = 2,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 5,
                    RoleId = 1,
                    MenuId = 3,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 6,
                    RoleId = 1,
                    MenuId = 3,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 7,
                    RoleId = 1,
                    MenuId = 3,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 8,
                    RoleId = 1,
                    MenuId = 3,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 9,
                    RoleId = 1,
                    MenuId = 4,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 10,
                    RoleId = 1,
                    MenuId = 4,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 11,
                    RoleId = 1,
                    MenuId = 4,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 12,
                    RoleId = 1,
                    MenuId = 4,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 13,
                    RoleId = 1,
                    MenuId = 5,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 14,
                    RoleId = 1,
                    MenuId = 5,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 15,
                    RoleId = 1,
                    MenuId = 5,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 16,
                    RoleId = 1,
                    MenuId = 5,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 17,
                    RoleId = 1,
                    MenuId = 6,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 18,
                    RoleId = 1,
                    MenuId = 6,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 19,
                    RoleId = 1,
                    MenuId = 6,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 20,
                    RoleId = 1,
                    MenuId = 6,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 21,
                    RoleId = 1,
                    MenuId = 6,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 22,
                    RoleId = 1,
                    MenuId = 6,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 23,
                    RoleId = 1,
                    MenuId = 7,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 24,
                    RoleId = 1,
                    MenuId = 7,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 25,
                    RoleId = 1,
                    MenuId = 7,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 26,
                    RoleId = 1,
                    MenuId = 7,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 27,
                    RoleId = 1,
                    MenuId = 8,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 28,
                    RoleId = 1,
                    MenuId = 8,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 29,
                    RoleId = 1,
                    MenuId = 8,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 30,
                    RoleId = 1,
                    MenuId = 8,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 31,
                    RoleId = 1,
                    MenuId = 8,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 32,
                    RoleId = 1,
                    MenuId = 8,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 33,
                    RoleId = 1,
                    MenuId = 9,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 34,
                    RoleId = 1,
                    MenuId = 9,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 35,
                    RoleId = 1,
                    MenuId = 9,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 36,
                    RoleId = 1,
                    MenuId = 9,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 37,
                    RoleId = 1,
                    MenuId = 9,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 38,
                    RoleId = 1,
                    MenuId = 9,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 39,
                    RoleId = 3,
                    MenuId = 11,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 40,
                    RoleId = 3,
                    MenuId = 11,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 41,
                    RoleId = 3,
                    MenuId = 11,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 42,
                    RoleId = 3,
                    MenuId = 11,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 43,
                    RoleId = 3,
                    MenuId = 11,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 44,
                    RoleId = 3,
                    MenuId = 11,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 45,
                    RoleId = 3,
                    MenuId = 12,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 46,
                    RoleId = 3,
                    MenuId = 12,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 47,
                    RoleId = 3,
                    MenuId = 12,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 48,
                    RoleId = 3,
                    MenuId = 12,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 49,
                    RoleId = 3,
                    MenuId = 13,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 50,
                    RoleId = 3,
                    MenuId = 13,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 51,
                    RoleId = 3,
                    MenuId = 13,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 52,
                    RoleId = 3,
                    MenuId = 13,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 53,
                    RoleId = 3,
                    MenuId = 14,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 54,
                    RoleId = 3,
                    MenuId = 14,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 55,
                    RoleId = 3,
                    MenuId = 14,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 56,
                    RoleId = 3,
                    MenuId = 14,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 57,
                    RoleId = 3,
                    MenuId = 16,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 58,
                    RoleId = 3,
                    MenuId = 17,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 59,
                    RoleId = 3,
                    MenuId = 17,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 60,
                    RoleId = 3,
                    MenuId = 17,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 61,
                    RoleId = 3,
                    MenuId = 17,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 62,
                    RoleId = 3,
                    MenuId = 17,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 63,
                    RoleId = 3,
                    MenuId = 17,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 64,
                    RoleId = 3,
                    MenuId = 18,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 65,
                    RoleId = 3,
                    MenuId = 18,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 66,
                    RoleId = 3,
                    MenuId = 18,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 67,
                    RoleId = 3,
                    MenuId = 18,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 68,
                    RoleId = 3,
                    MenuId = 18,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 69,
                    RoleId = 3,
                    MenuId = 18,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 70,
                    RoleId = 3,
                    MenuId = 19,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 71,
                    RoleId = 3,
                    MenuId = 19,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 72,
                    RoleId = 3,
                    MenuId = 19,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 73,
                    RoleId = 3,
                    MenuId = 19,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 74,
                    RoleId = 3,
                    MenuId = 19,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 75,
                    RoleId = 3,
                    MenuId = 19,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 76,
                    RoleId = 4,
                    MenuId = 16,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 77,
                    RoleId = 4,
                    MenuId = 17,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 78,
                    RoleId = 4,
                    MenuId = 17,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 79,
                    RoleId = 4,
                    MenuId = 17,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 80,
                    RoleId = 4,
                    MenuId = 17,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 81,
                    RoleId = 4,
                    MenuId = 17,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 82,
                    RoleId = 4,
                    MenuId = 17,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 83,
                    RoleId = 4,
                    MenuId = 18,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 84,
                    RoleId = 4,
                    MenuId = 18,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 85,
                    RoleId = 4,
                    MenuId = 18,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 86,
                    RoleId = 4,
                    MenuId = 18,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 87,
                    RoleId = 4,
                    MenuId = 18,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 88,
                    RoleId = 4,
                    MenuId = 18,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 89,
                    RoleId = 4,
                    MenuId = 19,
                    PermissionId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 90,
                    RoleId = 4,
                    MenuId = 19,
                    PermissionId = 2,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 91,
                    RoleId = 4,
                    MenuId = 19,
                    PermissionId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 92,
                    RoleId = 4,
                    MenuId = 19,
                    PermissionId = 4,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 93,
                    RoleId = 4,
                    MenuId = 19,
                    PermissionId = 5,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new RoleMenuPermission
                {
                    Id = 94,
                    RoleId = 4,
                    MenuId = 19,
                    PermissionId = 6,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                }
            );

            builder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = 1,
                    UserId = 1,
                    RoleId = 1,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new UserRole
                {
                    Id = 2,
                    UserId = 2,
                    RoleId = 3,
                    CreatedAt = StaticCreatedAt,
                    UpdatedAt = StaticCreatedAt,
                    CreatedBy = "system",
                    UpdatedBy = "system",
                    IsActive = true
                },
                new UserRole
                {
                    Id = 3,
                    UserId = 1,
                    RoleId = 3,
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