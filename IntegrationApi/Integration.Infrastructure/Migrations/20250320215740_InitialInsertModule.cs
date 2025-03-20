using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialInsertModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Modules",
                columns: new[] { "Id", "ApplicationId", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, "MOD0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Aplicaciones", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 2, 1, "MOD0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Módulos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 3, 1, "MOD0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Permisos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 4, 1, "MOD0000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Roles", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 5, 1, "MOD0000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Usuarios", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 6, 1, "MOD0000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Asignación de Permisos por Rol", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 7, 1, "MOD0000007", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Asignación de Roles por Usuario", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" }
                });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
