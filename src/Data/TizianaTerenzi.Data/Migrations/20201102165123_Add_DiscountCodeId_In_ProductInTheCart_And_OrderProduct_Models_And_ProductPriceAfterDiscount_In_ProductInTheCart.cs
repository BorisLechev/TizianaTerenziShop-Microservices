using Microsoft.EntityFrameworkCore.Migrations;

namespace TizianaTerenzi.Data.Migrations
{
    public partial class Add_DiscountCodeId_In_ProductInTheCart_And_OrderProduct_Models_And_ProductPriceAfterDiscount_In_ProductInTheCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProductPriceAfterDiscount",
                table: "ProductsInTheCart",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPriceAfterDiscount",
                table: "ProductsInTheCart");
        }
    }
}
