using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarsProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CarModelAbrv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abrv",
                table: "CarModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abrv",
                table: "CarModels");
        }
    }
}
