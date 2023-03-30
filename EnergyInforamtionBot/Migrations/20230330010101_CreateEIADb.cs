using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyInforamtionBot.Migrations
{
    /// <inheritdoc />
    public partial class CreateEIADb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeriesItems",
                columns: table => new
                {
                    Period = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesItems", x => x.Period);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeriesItems");
        }
    }
}
