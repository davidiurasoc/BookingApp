using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingApp_v2.Data.Migrations
{
    public partial class ChangedTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_ApprovedById",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_ApprovedById",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "LeaveRequests");

            migrationBuilder.AlterColumn<string>(
                name: "NumberOfDays",
                table: "LeaveAllocations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "LeaveRequests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedById",
                table: "LeaveRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfDays",
                table: "LeaveAllocations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_ApprovedById",
                table: "LeaveRequests",
                column: "ApprovedById");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_ApprovedById",
                table: "LeaveRequests",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
