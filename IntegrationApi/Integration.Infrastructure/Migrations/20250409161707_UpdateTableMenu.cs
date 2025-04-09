using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "Route",
                schema: "Security",
                table: "Menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Security",
                table: "Menu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                schema: "Security",
                table: "Menu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Security",
                table: "Menu",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedBy", "Name", "UpdatedBy" },
                values: new object[] { "system", "Configuración", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.CreateIndex(
                name: "IDX_Menus_Code",
                schema: "Security",
                table: "Menu",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IDX_Menus_Code",
                schema: "Security",
                table: "Menu");

            migrationBuilder.AlterColumn<string>(
                name: "Route",
                schema: "Security",
                table: "Menu",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Security",
                table: "Menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                schema: "Security",
                table: "Menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Security",
                table: "Menu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Applications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedBy", "Name", "UpdatedBy" },
                values: new object[] { "epulido", "Gestión de Aplicaciones", "epulido" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Modules",
                columns: new[] { "Id", "ApplicationId", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 2, 1, "MOD0000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Módulos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 3, 1, "MOD0000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Permisos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 4, 1, "MOD0000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Roles", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 5, 1, "MOD0000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Gestión de Usuarios", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 6, 1, "MOD0000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Asignación de Permisos por Rol", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" },
                    { 7, 1, "MOD0000007", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido", true, "Asignación de Roles por Usuario", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "epulido" }
                });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "epulido", "epulido" });
        }
    }
}
