using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practica5web.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenToMedicamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Medicamentos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Medicamentos");
        }
    }
}
