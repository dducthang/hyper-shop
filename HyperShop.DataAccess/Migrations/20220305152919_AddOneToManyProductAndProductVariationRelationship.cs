using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HyperShop.Data.Migrations
{
    public partial class AddOneToManyProductAndProductVariationRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Product_Id",
                table: "ProductVariations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariations_Product_Id",
                table: "ProductVariations",
                column: "Product_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariations_Products_Product_Id",
                table: "ProductVariations",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariations_Products_Product_Id",
                table: "ProductVariations");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariations_Product_Id",
                table: "ProductVariations");

            migrationBuilder.DropColumn(
                name: "Product_Id",
                table: "ProductVariations");
        }
    }
}
