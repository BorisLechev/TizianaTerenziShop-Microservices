using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MelegPerfumes.Data.Migrations
{
    public partial class Add_ProductInTheCart_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsInTheCart",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    IssuerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsInTheCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsInTheCart_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductsInTheCart_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInTheCart_IsDeleted",
                table: "ProductsInTheCart",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInTheCart_IssuerId",
                table: "ProductsInTheCart",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInTheCart_ProductId",
                table: "ProductsInTheCart",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsInTheCart");
        }
    }
}
