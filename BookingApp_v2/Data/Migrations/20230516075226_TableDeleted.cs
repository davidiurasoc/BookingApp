using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingApp_v2.Data.Migrations
{
    public partial class TableDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_RequestingEmployeeId",
                table: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "LeaveAllocations");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_RequestingEmployeeId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "RequestingEmployeeId",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<string>(
                name: "RequestingClientId",
                table: "LeaveRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_RequestingClientId",
                table: "LeaveRequests",
                column: "RequestingClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_RequestingClientId",
                table: "LeaveRequests",
                column: "RequestingClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_RequestingClientId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_RequestingClientId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "RequestingClientId",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<string>(
                name: "RequestingEmployeeId",
                table: "LeaveRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LeaveAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberOfDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveAllocations_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveAllocations_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_RequestingEmployeeId",
                table: "LeaveRequests",
                column: "RequestingEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAllocations_EmployeeId",
                table: "LeaveAllocations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAllocations_LeaveTypeId",
                table: "LeaveAllocations",
                column: "LeaveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_RequestingEmployeeId",
                table: "LeaveRequests",
                column: "RequestingEmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
