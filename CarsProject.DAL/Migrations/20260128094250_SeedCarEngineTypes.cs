using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarsProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedCarEngineTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CarEngineType",
                columns: new[] { "Id", "Abrv", "Type" },
                values: new object[,]
                {
                    { 1, "PET", "Petrol" },
                    { 2, "DSL", "Diesel" },
                    { 3, "ELE", "Electric" },
                    { 4, "HYB", "Hybrid" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarEngineType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CarEngineType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CarEngineType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CarEngineType",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
