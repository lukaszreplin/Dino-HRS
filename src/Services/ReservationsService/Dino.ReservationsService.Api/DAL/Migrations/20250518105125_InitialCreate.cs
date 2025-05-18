using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dino.ReservationsService.Api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CheckIn = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckOut = table.Column<DateOnly>(type: "date", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    GuestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CheckIn", "CheckOut", "GuestId", "RoomId", "Status" },
                values: new object[,]
                {
                    { new Guid("39ae7df2-8595-40f8-b611-f6558fa55739"), new DateOnly(2025, 5, 18), new DateOnly(2025, 5, 21), new Guid("ad9f0527-18f6-4485-9ee9-df9637c34f03"), new Guid("0dde4b79-7674-4b5d-bb77-88dcced2712a"), "Confirmed" },
                    { new Guid("7ad8df20-6cf8-4c3a-ad44-ae2a655a4453"), new DateOnly(2025, 5, 23), new DateOnly(2025, 5, 28), new Guid("f354fd2a-08ce-476d-a6c5-c72aee917e0e"), new Guid("62d0d84b-fb81-41fd-bcae-573393db6fd0"), "Confirmed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomId_CheckIn_CheckOut",
                table: "Reservations",
                columns: new[] { "RoomId", "CheckIn", "CheckOut" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
