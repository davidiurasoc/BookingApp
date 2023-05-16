using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingApp_v2.Data.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "LeaveTypeId",
                table: "RoomBookings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId",
                table: "RoomBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
