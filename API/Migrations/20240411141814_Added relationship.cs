using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Addedrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AirportId",
                table: "TAF",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AirportId",
                table: "METAR",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TAF_AirportId",
                table: "TAF",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_METAR_AirportId",
                table: "METAR",
                column: "AirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_METAR_Airport_AirportId",
                table: "METAR",
                column: "AirportId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TAF_Airport_AirportId",
                table: "TAF",
                column: "AirportId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_METAR_Airport_AirportId",
                table: "METAR");

            migrationBuilder.DropForeignKey(
                name: "FK_TAF_Airport_AirportId",
                table: "TAF");

            migrationBuilder.DropIndex(
                name: "IX_TAF_AirportId",
                table: "TAF");

            migrationBuilder.DropIndex(
                name: "IX_METAR_AirportId",
                table: "METAR");

            migrationBuilder.DropColumn(
                name: "AirportId",
                table: "TAF");

            migrationBuilder.DropColumn(
                name: "AirportId",
                table: "METAR");
        }
    }
}
