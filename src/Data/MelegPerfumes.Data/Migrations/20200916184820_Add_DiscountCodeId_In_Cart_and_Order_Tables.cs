namespace MelegPerfumes.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Add_DiscountCodeId_In_Cart_and_Order_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeId",
                table: "ProductsInTheCart",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Discount = table.Column<double>(nullable: false),
                    ExpiresOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_IsDeleted",
                table: "DiscountCodes",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "ProductsInTheCart");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Orders");
        }
    }
}
