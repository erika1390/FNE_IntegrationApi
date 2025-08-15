using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PermisosRolAsignador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "RoleMenuPermission",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "MenuId", "PermissionId", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 76, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 16, 1, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 77, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 17, 1, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 78, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 17, 2, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 79, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 17, 3, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 80, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 17, 4, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 81, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 17, 5, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 82, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 17, 6, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 82);
        }
    }
}
