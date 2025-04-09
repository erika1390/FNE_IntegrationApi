using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoleMenuPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleModulePermissions",
                schema: "Security");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleModulePermissions",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
        }
    }
}
