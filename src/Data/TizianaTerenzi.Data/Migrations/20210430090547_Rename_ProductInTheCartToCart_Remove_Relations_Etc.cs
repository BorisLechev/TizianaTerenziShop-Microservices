namespace TizianaTerenzi.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Rename_ProductInTheCartToCart_Remove_Relations_Etc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_AspNetUsers_UserId",
                table: "OrderProducts");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrderProducts_DiscountCodes_DiscountCodeId",
            //    table: "OrderProducts");

            migrationBuilder.DropTable(
                name: "ProductsInTheCart");

            //migrationBuilder.DropIndex(
            //    name: "IX_OrderProducts_DiscountCodeId",
            //    table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_UserId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderProducts");

            migrationBuilder.RenameColumn(
                name: "PriceWithDiscount",
                table: "Products",
                newName: "PriceWithGeneralDiscount");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductPriceWithDiscountCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiscountCodeId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Carts_DiscountCodes_DiscountCodeId",
                        column: x => x.DiscountCodeId,
                        principalTable: "DiscountCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_DiscountCodeId",
                table: "Carts",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_IsDeleted",
                table: "Carts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.RenameColumn(
                name: "PriceWithGeneralDiscount",
                table: "Products",
                newName: "PriceWithDiscount");

            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeId",
                table: "OrderProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OrderProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductsInTheCart",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountCodeId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductPriceAfterDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsInTheCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsInTheCart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductsInTheCart_DiscountCodes_DiscountCodeId",
                        column: x => x.DiscountCodeId,
                        principalTable: "DiscountCodes",
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
                name: "IX_OrderProducts_DiscountCodeId",
                table: "OrderProducts",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_UserId",
                table: "OrderProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInTheCart_DiscountCodeId",
                table: "ProductsInTheCart",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInTheCart_IsDeleted",
                table: "ProductsInTheCart",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInTheCart_ProductId",
                table: "ProductsInTheCart",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInTheCart_UserId",
                table: "ProductsInTheCart",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_AspNetUsers_UserId",
                table: "OrderProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_DiscountCodes_DiscountCodeId",
                table: "OrderProducts",
                column: "DiscountCodeId",
                principalTable: "DiscountCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
