using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HyperShop.Data.Migrations
{
    public partial class RemoveProductVarColumnThenAddProductAndColorColumnToImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ProductVariations_ProductVariation_Id",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ProductVariation_Id",
                table: "Images",
                newName: "Product_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ProductVariation_Id",
                table: "Images",
                newName: "IX_Images_Product_Id");

            migrationBuilder.AddColumn<int>(
                name: "Color_Id",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_Color_Id",
                table: "Images",
                column: "Color_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Colors_Color_Id",
                table: "Images",
                column: "Color_Id",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_Product_Id",
                table: "Images",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Colors_Color_Id",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_Product_Id",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_Color_Id",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Color_Id",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "Product_Id",
                table: "Images",
                newName: "ProductVariation_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Images_Product_Id",
                table: "Images",
                newName: "IX_Images_ProductVariation_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ProductVariations_ProductVariation_Id",
                table: "Images",
                column: "ProductVariation_Id",
                principalTable: "ProductVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
