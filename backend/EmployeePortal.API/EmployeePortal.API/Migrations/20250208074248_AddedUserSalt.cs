using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeePortal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_UserName",
                table: "Users",
                column: "UserName");

            migrationBuilder.CreateTable(
                name: "UserSalts",
                columns: table => new
                {
                    UserSaltId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSalts", x => x.UserSaltId);
                    table.ForeignKey(
                        name: "FK_UserSalts_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSalts_UserName",
                table: "UserSalts",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSalts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_UserName",
                table: "Users");
        }
    }
}
