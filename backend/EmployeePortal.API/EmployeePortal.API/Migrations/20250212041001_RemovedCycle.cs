using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeePortal.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_Users_UserId",
                table: "UserDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupMembers_Users_UserId",
                table: "UserGroupMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_Users_UserId",
                table: "UserDepartments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupMembers_Users_UserId",
                table: "UserGroupMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
