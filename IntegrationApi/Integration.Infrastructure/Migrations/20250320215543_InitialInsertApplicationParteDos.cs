using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialInsertApplicationParteDos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Applications",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 2, "APP0000002", new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Saga 2.0", new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 3, "APP0000003", new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Sicof Lite", new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "epulido" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
