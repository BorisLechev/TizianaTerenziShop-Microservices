namespace TizianaTerenzi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Remove_Non_Clustered_Index_SearchText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_SearchText",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "SearchText",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SearchText",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SearchText",
                table: "Products",
                column: "SearchText");
        }
    }
}
