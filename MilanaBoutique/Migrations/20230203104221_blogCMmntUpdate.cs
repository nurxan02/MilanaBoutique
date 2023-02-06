using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class blogCMmntUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BlogComments");
        }
    }
}
