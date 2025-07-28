using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CargueDataUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Code", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UpdatedBy", "UserName" },
                values: new object[] { 2, 0, "USR0000002", "b69f36df-8915-4287-949e-80c1f0d99cf8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "sjmedina@Minsalud.gov.co", false, "Sandra", true, "Medina", true, null, "SJMEDINA", "SJMEDINA@MINSALUD.GOV.CO", "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==", "3157234494", false, "2756991d-795c-4132-8848-34d79e60b300", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "sjmedina" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
