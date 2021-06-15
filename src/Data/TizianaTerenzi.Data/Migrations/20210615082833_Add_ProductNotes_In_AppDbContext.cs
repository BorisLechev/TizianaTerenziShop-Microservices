using Microsoft.EntityFrameworkCore.Migrations;

namespace TizianaTerenzi.Data.Migrations
{
    public partial class Add_ProductNotes_In_AppDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductNote_Notes_NoteId",
                table: "ProductNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductNote_Products_ProductId",
                table: "ProductNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductNote",
                table: "ProductNote");

            migrationBuilder.RenameTable(
                name: "ProductNote",
                newName: "ProductNotes");

            migrationBuilder.RenameIndex(
                name: "IX_ProductNote_ProductId",
                table: "ProductNotes",
                newName: "IX_ProductNotes_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductNote_NoteId",
                table: "ProductNotes",
                newName: "IX_ProductNotes_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductNote_IsDeleted",
                table: "ProductNotes",
                newName: "IX_ProductNotes_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductNotes",
                table: "ProductNotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductNotes_Notes_NoteId",
                table: "ProductNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_ProductNotes_Notes_NoteId",
                table: "ProductNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductNotes_Products_ProductId",
                table: "ProductNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductNotes",
                table: "ProductNotes");

            migrationBuilder.RenameTable(
                name: "ProductNotes",
                newName: "ProductNote");

            migrationBuilder.RenameIndex(
                name: "IX_ProductNotes_ProductId",
                table: "ProductNote",
                newName: "IX_ProductNote_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductNotes_NoteId",
                table: "ProductNote",
                newName: "IX_ProductNote_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductNotes_IsDeleted",
                table: "ProductNote",
                newName: "IX_ProductNote_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductNote",
                table: "ProductNote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductNote_Notes_NoteId",
                table: "ProductNote",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductNote_Products_ProductId",
                table: "ProductNote",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
