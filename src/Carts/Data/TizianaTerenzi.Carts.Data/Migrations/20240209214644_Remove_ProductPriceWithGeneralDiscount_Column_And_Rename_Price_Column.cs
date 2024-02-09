#nullable disable

namespace TizianaTerenzi.Carts.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class Remove_ProductPriceWithGeneralDiscount_Column_And_Rename_Price_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceWithDiscountCode",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ProductPriceWithGeneralDiscount",
                table: "Carts",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Carts",
                newName: "ProductPriceWithGeneralDiscount");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceWithDiscountCode",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
