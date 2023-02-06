using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class settingsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LittleWishlist",
                table: "Settings",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneIcon",
                table: "Settings",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserIcon",
                table: "Settings",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LittleWishlist",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PhoneIcon",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "UserIcon",
                table: "Settings");
        }
    }
}
