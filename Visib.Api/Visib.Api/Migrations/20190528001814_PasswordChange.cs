using Microsoft.EntityFrameworkCore.Migrations;

namespace Visib.Api.Migrations
{
    public partial class PasswordChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSetPassword",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSetPassword",
                table: "AspNetUsers");
        }
    }
}
