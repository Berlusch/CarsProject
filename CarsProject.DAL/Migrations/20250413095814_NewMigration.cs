using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarsProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarMakeId",
                table: "CarModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_CarMakeId",
                table: "CarModels",
                column: "CarMakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarMakes_CarMakeId",
                table: "CarModels",
                column: "CarMakeId",
                principalTable: "CarMakes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarMakes_CarMakeId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_CarMakeId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "CarMakeId",
                table: "CarModels");
        }
    }
}
