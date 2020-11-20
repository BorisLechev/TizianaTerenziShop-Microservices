using Microsoft.EntityFrameworkCore.Migrations;

namespace MelegPerfumes.Data.Migrations
{
    public partial class Add_User_In_OrderProduct_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OrderProducts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_UserId",
                table: "OrderProducts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_AspNetUsers_UserId",
                table: "OrderProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_AspNetUsers_UserId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_UserId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderProducts");
        }
    }
}
