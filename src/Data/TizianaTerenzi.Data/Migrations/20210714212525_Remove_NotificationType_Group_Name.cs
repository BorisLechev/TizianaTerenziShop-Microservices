namespace TizianaTerenzi.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Remove_NotificationType_Group_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_NotificationTypes_TypeId",
                table: "UserNotifications");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_TypeId",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ChatGroups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "UserNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ChatGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_TypeId",
                table: "UserNotifications",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_NotificationTypes_TypeId",
                table: "UserNotifications",
                column: "TypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
