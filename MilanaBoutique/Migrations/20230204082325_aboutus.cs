using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class aboutus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Banner = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    SubTitle = table.Column<string>(maxLength: 50, nullable: false),
                    Vision = table.Column<string>(maxLength: 250, nullable: false),
                    Mission = table.Column<string>(maxLength: 250, nullable: false),
                    NewsLink = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUs");
        }
    }
}
