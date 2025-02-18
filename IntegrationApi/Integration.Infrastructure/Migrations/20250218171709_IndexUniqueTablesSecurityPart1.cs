using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndexUniqueTablesSecurityPart1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Code",
                schema: "Security",
                table: "Users",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Code",
                schema: "Security",
                table: "Roles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                schema: "Security",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Code",
                schema: "Security",
                table: "Modules",
                column: "Code",
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Code",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Code",
                schema: "Security",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Code",
                schema: "Security",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Modules_Code",
                schema: "Security",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Code",
                schema: "Security",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Name",
                schema: "Security",
                table: "Applications");
        }
    }
}
