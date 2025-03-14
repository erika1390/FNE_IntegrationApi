using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TableRoleModulePermissions2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModules_Modules_ModuleId",
                schema: "Security",
                table: "RoleModules");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleModules_Permissions_PermissionId",
                schema: "Security",
                table: "RoleModules");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleModules_Roles_RoleId",
                schema: "Security",
                table: "RoleModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleModules",
                schema: "Security",
                table: "RoleModules");

            migrationBuilder.RenameTable(
                name: "RoleModules",
                schema: "Security",
                newName: "RoleModulePermissions",
                newSchema: "Security");

            migrationBuilder.RenameIndex(
                name: "IX_RoleModules_PermissionId",
                schema: "Security",
                table: "RoleModulePermissions",
                newName: "IX_RoleModulePermissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleModules_ModuleId",
                schema: "Security",
                table: "RoleModulePermissions",
                newName: "IX_RoleModulePermissions_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleModulePermissions",
                schema: "Security",
                table: "RoleModulePermissions",
                columns: new[] { "RoleId", "ModuleId", "PermissionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulePermissions_Modules_ModuleId",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "ModuleId",
                principalSchema: "Security",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulePermissions_Permissions_PermissionId",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "PermissionId",
                principalSchema: "Security",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulePermissions_Roles_RoleId",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "RoleId",
                principalSchema: "Security",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulePermissions_Modules_ModuleId",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulePermissions_Permissions_PermissionId",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulePermissions_Roles_RoleId",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleModulePermissions",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.RenameTable(
                name: "RoleModulePermissions",
                schema: "Security",
                newName: "RoleModules",
                newSchema: "Security");

            migrationBuilder.RenameIndex(
                name: "IX_RoleModulePermissions_PermissionId",
                schema: "Security",
                table: "RoleModules",
                newName: "IX_RoleModules_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleModulePermissions_ModuleId",
                schema: "Security",
                table: "RoleModules",
                newName: "IX_RoleModules_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleModules",
                schema: "Security",
                table: "RoleModules",
                columns: new[] { "RoleId", "ModuleId", "PermissionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModules_Modules_ModuleId",
                schema: "Security",
                table: "RoleModules",
                column: "ModuleId",
                principalSchema: "Security",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModules_Permissions_PermissionId",
                schema: "Security",
                table: "RoleModules",
                column: "PermissionId",
                principalSchema: "Security",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModules_Roles_RoleId",
                schema: "Security",
                table: "RoleModules",
                column: "RoleId",
                principalSchema: "Security",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
