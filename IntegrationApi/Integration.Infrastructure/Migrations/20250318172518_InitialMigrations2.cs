using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId1",
                schema: "Security",
                table: "RoleModulePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermissionId1",
                schema: "Security",
                table: "RoleModulePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId1",
                schema: "Security",
                table: "RoleModulePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleModulePermissions_ModuleId1",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "ModuleId1");

            migrationBuilder.CreateIndex(
                name: "IX_RoleModulePermissions_PermissionId1",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "PermissionId1");

            migrationBuilder.CreateIndex(
                name: "IX_RoleModulePermissions_RoleId1",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulePermissions_Modules_ModuleId1",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "ModuleId1",
                principalSchema: "Security",
                principalTable: "Modules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulePermissions_Permissions_PermissionId1",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "PermissionId1",
                principalSchema: "Security",
                principalTable: "Permissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulePermissions_Roles_RoleId1",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "RoleId1",
                principalSchema: "Security",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulePermissions_Modules_ModuleId1",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulePermissions_Permissions_PermissionId1",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulePermissions_Roles_RoleId1",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RoleModulePermissions_ModuleId1",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RoleModulePermissions_PermissionId1",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RoleModulePermissions_RoleId1",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropColumn(
                name: "ModuleId1",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropColumn(
                name: "PermissionId1",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                schema: "Security",
                table: "RoleModulePermissions");
        }
    }
}
