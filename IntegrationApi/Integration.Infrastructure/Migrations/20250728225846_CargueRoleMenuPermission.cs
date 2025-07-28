using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CargueRoleMenuPermission : Migration
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
                    { 39, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 11, 1, 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 40, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 11, 2, 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 41, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 11, 3, 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 42, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 11, 4, 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 43, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 11, 5, 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 44, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 11, 6, 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 44);
        }
    }
}
