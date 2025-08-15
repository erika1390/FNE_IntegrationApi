using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustesRolesSicofLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ApplicationId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ApplicationId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ApplicationId", "Name", "NormalizedName" },
                values: new object[] { 3, "Consultor", "CONSULTOR" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ApplicationId", "Code", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "IsActive", "Name", "NormalizedName", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 7, 3, "ROL0000007", "7f2a6c54-2d1e-4b37-9a5c-8f0e3b6a21d4", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", true, "QuienRevisa", "QUIENREVISA", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ApplicationId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ApplicationId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ApplicationId", "Name", "NormalizedName" },
                values: new object[] { 2, "Notificador", "NOTIFICADOR" });
        }
    }
}
