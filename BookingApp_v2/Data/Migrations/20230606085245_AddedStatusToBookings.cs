using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingApp_v2.Data.Migrations
{
    public partial class AddedStatusToBookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RoomBookings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoomBookings");
        }
    }
}
