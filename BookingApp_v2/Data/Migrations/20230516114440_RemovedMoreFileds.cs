using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingApp_v2.Data.Migrations
{
    public partial class RemovedMoreFileds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBookings_AspNetUsers_RequestingClientId",
                table: "RoomBookings");

            migrationBuilder.DropIndex(
                name: "IX_RoomBookings_RequestingClientId",
                table: "RoomBookings");

            migrationBuilder.DropColumn(
                name: "RequestingClientId",
                table: "RoomBookings");

            migrationBuilder.AddColumn<string>(
                name: "BookingClientId",
                table: "RoomBookings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomBookings_BookingClientId",
                table: "RoomBookings",
                column: "BookingClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBookings_AspNetUsers_BookingClientId",
                table: "RoomBookings",
                column: "BookingClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBookings_AspNetUsers_BookingClientId",
                table: "RoomBookings");

            migrationBuilder.DropIndex(
                name: "IX_RoomBookings_BookingClientId",
                table: "RoomBookings");

            migrationBuilder.DropColumn(
                name: "BookingClientId",
                table: "RoomBookings");

            migrationBuilder.AddColumn<string>(
                name: "RequestingClientId",
                table: "RoomBookings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomBookings_RequestingClientId",
                table: "RoomBookings",
                column: "RequestingClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBookings_AspNetUsers_RequestingClientId",
                table: "RoomBookings",
                column: "RequestingClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
