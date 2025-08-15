using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PermisosRolAsignador2 : Migration
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
                    { 89, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 19, 1, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 90, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 19, 2, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 91, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 19, 3, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 92, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 19, 4, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 93, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 19, 5, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 94, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 19, 6, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 94);
        }
    }
}
