using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnIdTableUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Security",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });
        }
    }
}
