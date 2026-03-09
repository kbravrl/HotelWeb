using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeTaskRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousekeepingTasks_Employees_AssignedToEmployeeId",
                table: "HousekeepingTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_HousekeepingTasks_Employees_AssignedToEmployeeId",
                table: "HousekeepingTasks",
                column: "AssignedToEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousekeepingTasks_Employees_AssignedToEmployeeId",
                table: "HousekeepingTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_HousekeepingTasks_Employees_AssignedToEmployeeId",
                table: "HousekeepingTasks",
                column: "AssignedToEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
