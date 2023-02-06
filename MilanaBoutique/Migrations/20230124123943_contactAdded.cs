using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class contactAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerImage = table.Column<string>(nullable: true),
                    BannerTitle = table.Column<string>(maxLength: 100, nullable: false),
                    BannerDesc = table.Column<string>(maxLength: 100, nullable: false),
                    ContactInformation = table.Column<string>(maxLength: 100, nullable: false),
                    OfficeLocation = table.Column<string>(maxLength: 100, nullable: false),
                    LocationIcon = table.Column<string>(maxLength: 100, nullable: false),
                    Phone = table.Column<string>(maxLength: 100, nullable: false),
                    PhoneIcon = table.Column<string>(maxLength: 100, nullable: false),
                    Mail = table.Column<string>(maxLength: 100, nullable: false),
                    MailIcon = table.Column<string>(maxLength: 100, nullable: false),
                    OpenTime = table.Column<string>(maxLength: 100, nullable: false),
                    CloseTime = table.Column<string>(maxLength: 100, nullable: false),
                    OclockIcon = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}
