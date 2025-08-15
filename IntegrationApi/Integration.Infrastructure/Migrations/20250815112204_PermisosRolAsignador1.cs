using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PermisosRolAsignador1 : Migration
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
                    { 83, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 18, 1, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 84, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 18, 2, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 85, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 18, 3, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 86, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 18, 4, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 87, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 18, 5, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 88, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 18, 6, 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 88);
        }
    }
}
