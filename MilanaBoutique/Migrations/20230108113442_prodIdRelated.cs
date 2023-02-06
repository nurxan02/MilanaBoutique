using Microsoft.EntityFrameworkCore.Migrations;

namespace MilanaBoutique.Migrations
{
    public partial class prodIdRelated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizeColors_Products_ProductId",
                table: "ProductSizeColors");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductSizeColors",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizeColors_Products_ProductId",
                table: "ProductSizeColors",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizeColors_Products_ProductId",
                table: "ProductSizeColors");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductSizeColors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizeColors_Products_ProductId",
                table: "ProductSizeColors",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
