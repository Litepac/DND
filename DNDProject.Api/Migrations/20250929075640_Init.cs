using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DNDProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Material = table.Column<int>(type: "INTEGER", nullable: false),
                    SizeLiters = table.Column<int>(type: "INTEGER", nullable: false),
                    WeeklyAmountKg = table.Column<double>(type: "REAL", nullable: false),
                    LastFillPct = table.Column<double>(type: "REAL", nullable: true),
                    LastPickupDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PreferredPickupFrequencyDays = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Containers",
                columns: new[] { "Id", "CustomerId", "LastFillPct", "LastPickupDate", "Material", "PreferredPickupFrequencyDays", "SizeLiters", "Type", "WeeklyAmountKg" },
                values: new object[,]
                {
                    { 1, null, 82.0, null, 1, 14, 2500, "Plast", 120.0 },
                    { 2, null, 46.0, null, 2, 21, 7000, "Jern", 900.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Containers");
        }
    }
}
