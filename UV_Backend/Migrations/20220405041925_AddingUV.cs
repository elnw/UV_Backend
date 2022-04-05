using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UV_Backend.Migrations
{
    public partial class AddingUV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UVData",
                columns: table => new
                {
                    HourForecast = table.Column<DateTime>(nullable: false),
                    Uvi = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UVData", x => x.HourForecast);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UVData");
        }
    }
}
