using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class settingsCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<string>(maxLength: 50, nullable: false),
                    SearchIcon = table.Column<string>(maxLength: 50, nullable: false),
                    WishlistIcon = table.Column<string>(maxLength: 50, nullable: false),
                    BasketIcon = table.Column<string>(maxLength: 50, nullable: false),
                    HeaderLogoImage = table.Column<string>(nullable: true),
                    FooterLogoImage = table.Column<string>(nullable: true),
                    FbIcon = table.Column<string>(maxLength: 50, nullable: false),
                    FbLink = table.Column<string>(maxLength: 160, nullable: false),
                    InstaIcon = table.Column<string>(maxLength: 50, nullable: false),
                    InstaLink = table.Column<string>(maxLength: 160, nullable: false),
                    TwitIcon = table.Column<string>(maxLength: 50, nullable: false),
                    TwitLink = table.Column<string>(maxLength: 160, nullable: false),
                    YoutubeIcon = table.Column<string>(maxLength: 50, nullable: false),
                    YtLink = table.Column<string>(maxLength: 160, nullable: false),
                    PinterestIcon = table.Column<string>(maxLength: 50, nullable: false),
                    PinterestLink = table.Column<string>(maxLength: 160, nullable: false),
                    FooterDesc = table.Column<string>(maxLength: 160, nullable: false),
                    Copyright = table.Column<string>(maxLength: 160, nullable: false),
                    HomePageRefundIcon = table.Column<string>(maxLength: 50, nullable: false),
                    HomePageDeliveryIcon = table.Column<string>(maxLength: 50, nullable: false),
                    HomePagePaymentIcon = table.Column<string>(maxLength: 50, nullable: false),
                    HomePageSupportIcon = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
