namespace TizianaTerenzi.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Make_ContactFormEntry_Deletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ContactFormEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContactFormEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ContactFormEntries_IsDeleted",
                table: "ContactFormEntries",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactFormEntries_IsDeleted",
                table: "ContactFormEntries");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ContactFormEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ContactFormEntries");
        }
    }
}
