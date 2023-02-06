using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class blogcommentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "BlogComments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "BlogComments");
        }
    }
}
