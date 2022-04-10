using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HyperShop.Data.Migrations
{
    public partial class UpdateColumnsInOrderDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Carts_Cart_Id",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "Cart_Id",
                table: "OrderDetail",
                newName: "Order_Id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_Cart_Id",
                table: "OrderDetail",
                newName: "IX_OrderDetail_Order_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Orders_Order_Id",
                table: "OrderDetail",
                column: "Order_Id",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Orders_Order_Id",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "Order_Id",
                table: "OrderDetail",
                newName: "Cart_Id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_Order_Id",
                table: "OrderDetail",
                newName: "IX_OrderDetail_Cart_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Carts_Cart_Id",
                table: "OrderDetail",
                column: "Cart_Id",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
