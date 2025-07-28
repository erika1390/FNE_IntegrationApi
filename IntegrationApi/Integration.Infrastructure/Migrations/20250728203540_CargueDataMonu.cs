using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CargueDataMonu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Menu",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Icon", "IsActive", "ModuleId", "Name", "Order", "ParentMenuId", "Route", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "MNU0000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "cog-outline", true, 1, "Administración", 0, null, "/administracion", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 10, "MNU0000010", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "cog-outline", true, 2, "Administración", 0, null, "/administracion", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 15, "MNU0000015", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "description", true, 3, "Principal", 0, null, "/interface", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 2, "MNU0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "grid-outline", true, 1, "Gestionar Aplicación", 1, 1, "/administracion/gestionar-aplicacion", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 3, "MNU0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "layers-outline", true, 1, "Gestionar Módulo", 2, 1, "/administracion/gestionar-modulo", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 4, "MNU0000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "menu-outline", true, 1, "Gestionar Menú", 3, 1, "/administracion/gestionar-menu", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 5, "MNU0000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "lock-closed-outline", true, 1, "Gestionar Permisos", 4, 1, "/administracion/gestionar-permisos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 6, "MNU0000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "people-outline", true, 1, "Gestionar Usuarios", 5, 1, "/administracion/gestionar-usuarios", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 7, "MNU0000007", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "extension-puzzle-outline", true, 1, "Gestionar Roles", 6, 1, "/administracion/gestionar-roles", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 8, "MNU0000008", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "person-outline", true, 1, "Asignar Role a Usuario", 7, 1, "/administracion/asignar-roles", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 9, "MNU0000009", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "key-outline", true, 1, "Configuración de Permisos", 8, 1, "/administracion/configuracion-permisos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 11, "MNU0000011", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "people-outline", true, 2, "Gestionar Usuarios", 1, 10, "/administracion/gestionar-usuarios", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 12, "MNU0000012", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "extension-puzzle-outline", true, 2, "Gestionar Roles", 2, 10, "/administracion/gestionar-roles", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 13, "MNU0000013", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "person-outline", true, 2, "Asignar Role a Usuario", 3, 10, "/administracion/asignar-roles", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 14, "MNU0000014", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "key-outline", true, 2, "Configuración de Permisos", 4, 10, "/administracion/configuracion-permisos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 16, "MNU0000016", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "description", true, 3, "Panel de control", 1, 15, "/interface/inicio", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 17, "MNU0000017", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "description", true, 3, "Tramite", 2, 15, "/interface/tramites", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 18, "MNU0000018", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "description", true, 3, "Inscripciones", 3, 15, "/interface/inscripciones", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 19, "MNU0000019", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "description", true, 3, "Novedades", 4, 15, "/interface/novedades", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Menu",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
