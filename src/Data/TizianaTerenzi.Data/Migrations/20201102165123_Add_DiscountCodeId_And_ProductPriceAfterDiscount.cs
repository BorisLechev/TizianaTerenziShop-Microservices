namespace TizianaTerenzi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Add_DiscountCodeId_And_ProductPriceAfterDiscount : Migration
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
