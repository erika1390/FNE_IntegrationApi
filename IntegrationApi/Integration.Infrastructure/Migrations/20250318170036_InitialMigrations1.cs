using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulePermissions_Roles_RoleId",
                schema: "Security",
                table: "RoleModulePermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulePermissions_Roles_RoleId",
                schema: "Security",
                table: "RoleModulePermissions",
                column: "RoleId",
                principalSchema: "Security",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulePermissions_Roles_RoleId",
                schema: "Security",
                table: "RoleModulePermissions");

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
    }
}
