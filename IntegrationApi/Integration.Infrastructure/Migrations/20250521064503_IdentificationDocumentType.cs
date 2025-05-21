using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdentificationDocumentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Parametric");

            migrationBuilder.CreateTable(
                name: "IdentificationDocumentType",
                schema: "Parametric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentificationDocumentType", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Parametric",
                table: "IdentificationDocumentType",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "Description", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "CC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Cedula Ciudadania", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 2, "CE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Cedula Extrangeria", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 3, "PA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Pasaporte", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 4, "PE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permiso Especial de permanencia", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 5, "PT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permiso por protección temporal", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" },
                    { 6, "NI", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Nit", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentificationDocumentType",
                schema: "Parametric");
        }
    }
}
