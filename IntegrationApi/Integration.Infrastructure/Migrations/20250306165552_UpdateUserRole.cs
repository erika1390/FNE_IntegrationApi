using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Applications_ApplicationId",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_ApplicationId",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                schema: "Security",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                schema: "Security",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                schema: "Security",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId", "ApplicationId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ApplicationId",
                schema: "Security",
                table: "UserRoles",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Applications_ApplicationId",
                schema: "Security",
                table: "UserRoles",
                column: "ApplicationId",
                principalSchema: "Security",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
