using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "Audit");

            migrationBuilder.CreateTable(
                name: "Applications",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "Audit",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeApplication = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CodeUser = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Information"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Method = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationMs = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "Security",
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "Security",
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Security",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Security",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentMenuId = table.Column<int>(type: "int", nullable: true),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Route = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menu_Menu_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalSchema: "Security",
                        principalTable: "Menu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Menu_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Security",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleModulePermissions",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModulePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleModulePermissions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Security",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleModulePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Security",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleModulePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleMenuPermission",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenuPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleMenuPermission_Menu_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "Security",
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMenuPermission_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Security",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMenuPermission_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Applications",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "APP0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Integrador", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 2, "APP0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Saga 2.0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 3, "APP0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Sicof Lite", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Permissions",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "PER0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Consultar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 2, "PER0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Crear", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 3, "PER0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Modificar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 4, "PER0000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Desactivar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 5, "PER0000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Cargar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 6, "PER0000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Descargar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Code", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UpdatedBy", "UserName" },
                values: new object[] { 1, 0, "USR0000001", "b69f36df-8915-4287-949e-80c1f0d99cf8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido@minsalud.gov.co", false, "Erika", true, "Pulido", true, null, "EPULIDO", "EPULIDO@MINSALUD.GOV.CO", "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==", "3157234493", false, "2756991d-795c-4132-8848-34d79e60b300", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", "epulido" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Modules",
                columns: new[] { "Id", "ApplicationId", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, "MOD0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Aplicaciones", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 2, 1, "MOD0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Módulos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 3, 1, "MOD0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Permisos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 4, 1, "MOD0000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Roles", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 5, 1, "MOD0000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Usuarios", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 6, 1, "MOD0000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Asignación de Permisos por Rol", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 7, 1, "MOD0000007", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Asignación de Roles por Usuario", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Code",
                schema: "Security",
                table: "Applications",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Name",
                schema: "Security",
                table: "Applications",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ModuleId",
                schema: "Security",
                table: "Menu",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ParentMenuId",
                schema: "Security",
                table: "Menu",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IDX_Modules_Code",
                schema: "Security",
                table: "Modules",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ApplicationId",
                schema: "Security",
                table: "Modules",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IDX_Permissions_Code",
                schema: "Security",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Security",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenuPermission_MenuId",
                schema: "Security",
                table: "RoleMenuPermission",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenuPermission_PermissionId",
                schema: "Security",
                table: "RoleMenuPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenuPermission_RoleId",
                schema: "Security",
                table: "RoleMenuPermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IDX_RoleModules_RoleId_ModuleId_PermissionId",
                schema: "Security",
                table: "RoleModulePermissions",
                columns: new[] { "RoleId", "ModuleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleModulePermissions_ModuleId",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleModulePermissions_PermissionId",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IDX_Roles_Code",
                schema: "Security",
                table: "Roles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ApplicationId",
                schema: "Security",
                table: "Roles",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Security",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IDX_UserClaims_UserId",
                schema: "Security",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IDX_UserLogins_UserId",
                schema: "Security",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IDX_UserRoles_UserId_RoleId",
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Security",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Security",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IDX_Users_Code",
                schema: "Security",
                table: "Users",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_Users_Email",
                schema: "Security",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IDX_Users_UserName",
                schema: "Security",
                table: "Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Security",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "RoleMenuPermission",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "RoleModulePermissions",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Menu",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Applications",
                schema: "Security");
        }
    }
}
