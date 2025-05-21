using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Department1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Parametric",
                table: "Department",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Parametric",
                table: "Department",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Parametric",
                table: "Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Parametric",
                table: "Department",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "Parametric",
                table: "Department",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });

            migrationBuilder.UpdateData(
                schema: "Parametric",
                table: "Department",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Parametric",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Parametric",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Parametric",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "Parametric",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Parametric",
                table: "Department");
        }
    }
}
