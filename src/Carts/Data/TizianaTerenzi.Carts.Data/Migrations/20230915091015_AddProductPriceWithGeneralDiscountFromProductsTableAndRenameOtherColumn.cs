#nullable disable

namespace TizianaTerenzi.Carts.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddProductPriceWithGeneralDiscountFromProductsTableAndRenameOtherColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductPriceWithDiscountCode",
                table: "Carts",
                newName: "ProductPriceWithGeneralDiscount");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceWithDiscountCode",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceWithDiscountCode",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ProductPriceWithGeneralDiscount",
                table: "Carts",
                newName: "ProductPriceWithDiscountCode");
        }
    }
}
