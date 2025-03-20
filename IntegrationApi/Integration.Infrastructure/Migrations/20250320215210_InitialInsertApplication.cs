using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialInsertApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Applications",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, "APP0000001", new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Integrador", new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "epulido" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
