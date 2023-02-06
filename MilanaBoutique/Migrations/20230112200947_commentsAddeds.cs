using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class commentsAddeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Star",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Star",
                table: "Comments",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);
        }
    }
}
