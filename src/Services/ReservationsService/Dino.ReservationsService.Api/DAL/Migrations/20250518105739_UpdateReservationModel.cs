using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dino.ReservationsService.Api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("39ae7df2-8595-40f8-b611-f6558fa55739"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("7ad8df20-6cf8-4c3a-ad44-ae2a655a4453"));

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CheckIn", "CheckOut", "GuestId", "RoomId", "Status" },
                values: new object[,]
                {
                    { new Guid("aa5d6369-b31a-45d9-b39d-e1e2288818fa"), new DateOnly(2025, 5, 18), new DateOnly(2025, 5, 21), new Guid("fd0fb19a-8242-4c87-bb93-386e1a61552e"), new Guid("5df24c2f-3738-4920-a403-815dfa4a85dd"), "Confirmed" },
                    { new Guid("c2592960-d746-4309-954f-2c88c1d0dc21"), new DateOnly(2025, 5, 23), new DateOnly(2025, 5, 28), new Guid("f11bb0d5-31a3-4cc5-82f8-a2f224295d42"), new Guid("a2905bdd-34bb-45e2-b1cd-1180159fde08"), "Confirmed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("aa5d6369-b31a-45d9-b39d-e1e2288818fa"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("c2592960-d746-4309-954f-2c88c1d0dc21"));

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CheckIn", "CheckOut", "GuestId", "RoomId", "Status" },
                values: new object[,]
                {
                    { new Guid("39ae7df2-8595-40f8-b611-f6558fa55739"), new DateOnly(2025, 5, 18), new DateOnly(2025, 5, 21), new Guid("ad9f0527-18f6-4485-9ee9-df9637c34f03"), new Guid("0dde4b79-7674-4b5d-bb77-88dcced2712a"), "Confirmed" },
                    { new Guid("7ad8df20-6cf8-4c3a-ad44-ae2a655a4453"), new DateOnly(2025, 5, 23), new DateOnly(2025, 5, 28), new Guid("f354fd2a-08ce-476d-a6c5-c72aee917e0e"), new Guid("62d0d84b-fb81-41fd-bcae-573393db6fd0"), "Confirmed" }
                });
        }
    }
}
