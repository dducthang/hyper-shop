using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HyperShop.Data.Migrations
{
    public partial class AddReceiverColumnToOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "Orders");
        }
    }
}
