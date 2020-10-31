using Microsoft.EntityFrameworkCore.Migrations;

namespace MelegPerfumes.Data.Migrations
{
    public partial class Add_User_In_OrderProduct_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OrderProduct",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_UserId",
                table: "OrderProduct",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_AspNetUsers_UserId",
                table: "OrderProduct",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_AspNetUsers_UserId",
                table: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_OrderProduct_UserId",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderProduct");
        }
    }
}
