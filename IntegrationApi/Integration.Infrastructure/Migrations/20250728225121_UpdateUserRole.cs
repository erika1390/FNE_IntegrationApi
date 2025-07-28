using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "RoleId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 1, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", 1 },
                    { 2, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", 2 },
                    { 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, 3, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
