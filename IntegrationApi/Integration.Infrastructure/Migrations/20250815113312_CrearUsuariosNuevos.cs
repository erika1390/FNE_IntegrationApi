using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CrearUsuariosNuevos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Code", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UpdatedBy", "UserName" },
                values: new object[,]
                {
                    { 3, 0, "USR0000003", "b69f36df-8915-4287-949e-80c1f0d99cf8", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "egiraldo@Minsalud.gov.co", false, "Estefania", true, "Giraldo Chica", true, null, "EGIRALDO", "EGIRALDO@MINSALUD.GOV.CO", "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==", "3157234495", false, "2756991d-795c-4132-8848-34d79e60b300", false, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", "egiraldo" },
                    { 4, 0, "USR0000004", "b69f36df-8915-4287-949e-80c1f0d99cf8", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "jcuervo@Minsalud.gov.co", false, "Julian", true, "Cuervo Bustamante", true, null, "JCUERVO", "JCUERVO@MINSALUD.GOV.CO", "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==", "3157234496", false, "2756991d-795c-4132-8848-34d79e60b300", false, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", "jcuervo" },
                    { 5, 0, "USR0000005", "b69f36df-8915-4287-949e-80c1f0d99cf8", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "wmolina@Minsalud.gov.co", false, "William", true, "Molina Morales", true, null, "WMOLINA", "WMOLINA@MINSALUD.GOV.CO", "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==", "3157234497", false, "2756991d-795c-4132-8848-34d79e60b300", false, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", "wmolina" },
                    { 6, 0, "USR0000006", "b69f36df-8915-4287-949e-80c1f0d99cf8", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", new DateTime(1990, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "jvalenciar@Minsalud.gov.co", false, "Juan Felipe", true, "Valencia Renteria", true, null, "JVALENCIAR", "JVALENCIAR@MINSALUD.GOV.CO", "AQAAAAIAAYagAAAAEMorJok85V7Kpf/EgOzE6dsr3UWrk6idDyT7BZszoRpr9OziW0BLL6vuF2zVj0B5ig==", "3157234498", false, "2756991d-795c-4132-8848-34d79e60b300", false, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), "system", "jvalenciar" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
