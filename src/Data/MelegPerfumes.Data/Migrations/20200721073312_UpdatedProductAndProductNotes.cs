using Microsoft.EntityFrameworkCore.Migrations;

namespace MelegPerfumes.Data.Migrations
{
    public partial class UpdatedProductAndProductNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "ProductNotes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductNotes_ProductId",
                table: "ProductNotes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductNotes_Products_ProductId",
                table: "ProductNotes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductNotes_Products_ProductId",
                table: "ProductNotes");

            migrationBuilder.DropIndex(
                name: "IX_ProductNotes_ProductId",
                table: "ProductNotes");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductNotes");

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "ProductNotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
