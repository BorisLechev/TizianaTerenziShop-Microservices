namespace TizianaTerenzi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Change_Discount_Numeric_Type_To_Byte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_SearchText",
                table: "Products");

            migrationBuilder.AlterColumn<byte>(
                name: "Percent",
                table: "GeneralDiscounts",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Discount",
                table: "DiscountCodes",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SearchText",
                table: "Products",
                column: "SearchText");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_SearchText",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "Percent",
                table: "GeneralDiscounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "DiscountCodes",
                type: "float",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SearchText",
                table: "Products",
                column: "SearchText",
                unique: true,
                filter: "[SearchText] IS NOT NULL");
        }
    }
}
