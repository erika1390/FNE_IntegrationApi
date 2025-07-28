using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CargueInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "Parametric");

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
                name: "Department",
                schema: "Parametric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeDane = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentificationDocumentType",
                schema: "Parametric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentificationDocumentType", x => x.Id);
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
                name: "City",
                schema: "Parametric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CodeDane = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Parametric",
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Route = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    { 1, "APP0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Integrador", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 2, "APP0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Saga 2.0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 3, "APP0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Sicof Lite", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" }
                });

            migrationBuilder.InsertData(
                schema: "Parametric",
                table: "Department",
                columns: new[] { "Id", "CodeDane", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "91", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Amazonas", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 2, "05", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Antioquia", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 3, "81", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Arauca", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 4, "08", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Atlántico", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 5, "11", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Bogotá, D.C.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 6, "13", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Bolívar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 7, "15", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Boyacá", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 8, "17", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Caldas", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 9, "18", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Caquetá", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 10, "85", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Casanare", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 11, "19", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Cauca", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 12, "20", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Cesar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 13, "27", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Chocó", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 14, "23", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Córdoba", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 15, "25", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Cundinamarca", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 16, "94", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Guainía", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 17, "95", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Guaviare", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 18, "41", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Huila", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 19, "44", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "La Guajira", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 20, "47", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Magdalena", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 21, "50", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Meta", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 22, "52", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Nariño", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 23, "54", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Norte de Santander", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 24, "86", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Putumayo", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 25, "63", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Quindío", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 26, "66", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Risaralda", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 27, "88", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "San Andrés, Providencia y Santa Catalina", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 28, "68", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Santander", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 29, "70", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Sucre", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 30, "73", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Tolima", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 31, "76", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Valle del Cauca", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 32, "97", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Vaupés", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 33, "99", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Vichada", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" }
                });

            migrationBuilder.InsertData(
                schema: "Parametric",
                table: "IdentificationDocumentType",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "Description", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "CC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Cedula Ciudadania", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 2, "CE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Cedula Extrangeria", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 3, "PA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Pasaporte", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 4, "PE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permiso Especial de permanencia", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 5, "PT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permiso por protección temporal", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 6, "NI", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Nit", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Permissions",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "PER0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Consultar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 2, "PER0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Crear", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 3, "PER0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Modificar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 4, "PER0000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Desactivar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 5, "PER0000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Cargar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 6, "PER0000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Descargar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Code", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UpdatedBy", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "USR0000001", "b69f36df-8915-4287-949e-80c1f0d99cf8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido@minsalud.gov.co", false, "Erika", true, "Pulido", true, null, "EPULIDO", "EPULIDO@MINSALUD.GOV.CO", "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==", "3157234493", false, "2756991d-795c-4132-8848-34d79e60b300", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "epulido" },
                    { 2, 0, "USR0000002", "568e8e5e-9788-4b25-972f-e09d4d75836f", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "sjmedina@Minsalud.gov.co", false, "Sandra Julieth", true, "Medina Dominguez", false, null, "SJMEDINA@MINSALUD.GOV.CO", "SJMEDINA", "AQAAAAIAAYagAAAAEDsuejQaFVcndAo8Cvo/cl4aI6bcGE4IhBVxkoqtznSfUDQznAnpHK3pvYWGaTmqfA==", "3157234495", false, "0ca7f47b-1dd8-48fb-a762-0394a09384df", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "sjmedina" },
                    { 3, 0, "USR0000003", "f39feedf-4c86-44d5-8aac-f9df9416c0e2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "egiraldo@Minsalud.gov.co", false, "Estefania", true, "Giraldo Chica", false, null, "EGIRALDO@MINSALUD.GOV.CO", "EGIRALDO", "AQAAAAIAAYagAAAAEOlghBRGxy2MX46D7wN3cnRaLZO9/lbgw6MuYNjP/xKPfmfTRkyeJ6IUfu5Zh6WUcA==", "3157234496", false, "a9ce9e42-45e2-4f62-a0ef-9e19507161c7", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "egiraldo" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Modules",
                columns: new[] { "Id", "ApplicationId", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, "MOD0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Administración", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 2, 3, "MOD0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Administración", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 3, 3, "MOD0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, "Principal", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" }
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
                name: "IX_City_DepartmentId",
                schema: "Parametric",
                table: "City",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IDX_Menus_Code",
                schema: "Security",
                table: "Menu",
                column: "Code",
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
                name: "City",
                schema: "Parametric");

            migrationBuilder.DropTable(
                name: "IdentificationDocumentType",
                schema: "Parametric");

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
                name: "Department",
                schema: "Parametric");

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
