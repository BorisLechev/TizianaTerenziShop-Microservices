#nullable disable

namespace TizianaTerenzi.Notifications.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class Add_ApplicationUserNotification_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserNotifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverUsername = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserNotifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNotifications_IsDeleted",
                table: "ApplicationUserNotifications",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNotifications_ReceiverUsername",
                table: "ApplicationUserNotifications",
                column: "ReceiverUsername");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNotifications_SenderId",
                table: "ApplicationUserNotifications",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserNotifications");
        }
    }
}
