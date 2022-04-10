using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HyperShop.Data.Migrations
{
    public partial class AddOrderDetailToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVariation_Id = table.Column<int>(type: "int", nullable: false),
                    Cart_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Carts_Cart_Id",
                        column: x => x.Cart_Id,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_ProductVariations_ProductVariation_Id",
                        column: x => x.ProductVariation_Id,
                        principalTable: "ProductVariations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Cart_Id",
                table: "OrderDetail",
                column: "Cart_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductVariation_Id",
                table: "OrderDetail",
                column: "ProductVariation_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetail");
        }
    }
}
