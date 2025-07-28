using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CargueDataRoleMenuPermission : Migration
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
                    { 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 2, 1, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 2, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 2, 2, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 2, 3, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 4, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 2, 4, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 5, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 3, 1, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 6, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 3, 2, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 7, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 3, 3, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 8, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 3, 4, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 9, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 4, 1, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 10, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 4, 2, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 11, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 4, 3, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 12, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 4, 4, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 13, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 5, 1, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 14, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 5, 2, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 15, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 5, 3, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 16, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 5, 4, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 17, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 6, 1, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 18, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 6, 2, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 19, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 6, 3, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 20, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 6, 4, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 21, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 6, 5, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 22, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 6, 6, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 23, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 7, 1, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 24, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 7, 2, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 25, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 7, 3, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 26, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 7, 4, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 27, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 8, 1, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 28, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 8, 2, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 29, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 8, 3, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 30, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 8, 4, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 31, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 8, 5, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 32, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 8, 6, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 33, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 9, 1, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 34, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 9, 2, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 35, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 9, 3, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 36, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 9, 4, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 37, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 9, 5, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 38, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 9, 6, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "RoleMenuPermission",
                keyColumn: "Id",
                keyValue: 38);
        }
    }
}
