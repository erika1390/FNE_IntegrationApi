using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialInsertPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Permissions",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "PER0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Consultar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 2, "PER0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Crear", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 3, "PER0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Modificar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 4, "PER0000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Desactivar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 5, "PER0000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Cargar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 6, "PER0000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Descargar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
