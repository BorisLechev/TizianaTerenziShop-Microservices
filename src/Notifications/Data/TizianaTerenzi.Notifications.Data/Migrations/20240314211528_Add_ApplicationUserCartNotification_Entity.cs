#nullable disable

namespace TizianaTerenzi.Notifications.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class Add_ApplicationUserCartNotification_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserCartNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfProductsInTheUsersCart = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCartNotifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCartNotifications_IsDeleted",
                table: "ApplicationUserCartNotifications",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCartNotifications_UserId",
                table: "ApplicationUserCartNotifications",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCartNotifications");
        }
    }
}
