using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DNDProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PickupEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContainerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FillPct = table.Column<double>(type: "REAL", nullable: true),
                    WeightKg = table.Column<double>(type: "REAL", nullable: true),
                    DistanceKm = table.Column<double>(type: "REAL", nullable: true),
                    Co2Kg = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickupEvents_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PickupEvents",
                columns: new[] { "Id", "Co2Kg", "ContainerId", "DistanceKm", "FillPct", "Timestamp", "WeightKg" },
                values: new object[,]
                {
                    { 1, null, 1, null, 88.0, new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 110.0 },
                    { 2, null, 1, null, 92.0, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 125.0 },
                    { 3, null, 2, null, 51.0, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 480.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_CustomerId",
                table: "Containers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupEvents_ContainerId_Timestamp",
                table: "PickupEvents",
                columns: new[] { "ContainerId", "Timestamp" });

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Customers_CustomerId",
                table: "Containers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Customers_CustomerId",
                table: "Containers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "PickupEvents");

            migrationBuilder.DropIndex(
                name: "IX_Containers_CustomerId",
                table: "Containers");
        }
    }
}
