using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class statusAddedd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "MessageToAdmin",
                table: "Orders",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageToUser",
                table: "Orders",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageToAdmin",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MessageToUser",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
