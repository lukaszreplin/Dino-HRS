using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dino.ReservationsService.Api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("aa5d6369-b31a-45d9-b39d-e1e2288818fa"),
                column: "CheckOut",
                value: new DateOnly(2025, 5, 20));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("c2592960-d746-4309-954f-2c88c1d0dc21"),
                columns: new[] { "CheckIn", "CheckOut" },
                values: new object[] { new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 8) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("aa5d6369-b31a-45d9-b39d-e1e2288818fa"),
                column: "CheckOut",
                value: new DateOnly(2025, 5, 21));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("c2592960-d746-4309-954f-2c88c1d0dc21"),
                columns: new[] { "CheckIn", "CheckOut" },
                values: new object[] { new DateOnly(2025, 5, 23), new DateOnly(2025, 5, 28) });
        }
    }
}
