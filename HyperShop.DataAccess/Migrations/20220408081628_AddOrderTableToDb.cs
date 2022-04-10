using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HyperShop.Data.Migrations
{
    public partial class AddOrderTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status_Id = table.Column<int>(type: "int", nullable: false),
                    CityShipCost_Id = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_CityShipCosts_CityShipCost_Id",
                        column: x => x.CityShipCost_Id,
                        principalTable: "CityShipCosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatuses_Status_Id",
                        column: x => x.Status_Id,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CityShipCost_Id",
                table: "Orders",
                column: "CityShipCost_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status_Id",
                table: "Orders",
                column: "Status_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_User_Id",
                table: "Orders",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
