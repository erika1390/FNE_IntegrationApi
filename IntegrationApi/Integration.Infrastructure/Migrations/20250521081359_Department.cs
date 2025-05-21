using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Department : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                schema: "Parametric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeDane = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Parametric",
                table: "Department",
                columns: new[] { "Id", "CodeDane", "Name" },
                values: new object[,]
                {
                    { 1, "91", "Amazonas" },
                    { 2, "05", "Antioquia" },
                    { 3, "81", "Arauca" },
                    { 4, "08", "Atlántico" },
                    { 5, "11", "Bogotá, D.C." },
                    { 6, "13", "Bolívar" },
                    { 7, "15", "Boyacá" },
                    { 8, "17", "Caldas" },
                    { 9, "18", "Caquetá" },
                    { 10, "85", "Casanare" },
                    { 11, "19", "Cauca" },
                    { 12, "20", "Cesar" },
                    { 13, "27", "Chocó" },
                    { 14, "23", "Córdoba" },
                    { 15, "25", "Cundinamarca" },
                    { 16, "94", "Guainía" },
                    { 17, "95", "Guaviare" },
                    { 18, "41", "Huila" },
                    { 19, "44", "La Guajira" },
                    { 20, "47", "Magdalena" },
                    { 21, "50", "Meta" },
                    { 22, "52", "Nariño" },
                    { 23, "54", "Norte de Santander" },
                    { 24, "86", "Putumayo" },
                    { 25, "63", "Quindío" },
                    { 26, "66", "Risaralda" },
                    { 27, "88", "San Andrés, Providencia y Santa Catalina" },
                    { 28, "68", "Santander" },
                    { 29, "70", "Sucre" },
                    { 30, "73", "Tolima" },
                    { 31, "76", "Valle del Cauca" },
                    { 32, "97", "Vaupés" },
                    { 33, "99", "Vichada" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Department",
                schema: "Parametric");
        }
    }
}
