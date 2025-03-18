using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeePortal.API.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurationEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_ManagerId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId",
                table: "UserDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_Users_UserId",
                table: "UserDepartments");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "PerformanceMetrics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "PerformanceMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Competencies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Competencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceMetrics_ManagerId",
                table: "PerformanceMetrics",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Competencies_ManagerId",
                table: "Competencies",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competencies_Users_ManagerId",
                table: "Competencies",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_ManagerId",
                table: "Departments",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PerformanceMetrics_Users_ManagerId",
                table: "PerformanceMetrics",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId",
                table: "UserDepartments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_Users_UserId",
                table: "UserDepartments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competencies_Users_ManagerId",
                table: "Competencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_ManagerId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_PerformanceMetrics_Users_ManagerId",
                table: "PerformanceMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId",
                table: "UserDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartments_Users_UserId",
                table: "UserDepartments");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceMetrics_ManagerId",
                table: "PerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_Competencies_ManagerId",
                table: "Competencies");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "PerformanceMetrics");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "PerformanceMetrics");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Competencies");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Competencies");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_ManagerId",
                table: "Departments",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_Departments_DepartmentId",
                table: "UserDepartments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartments_Users_UserId",
                table: "UserDepartments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
