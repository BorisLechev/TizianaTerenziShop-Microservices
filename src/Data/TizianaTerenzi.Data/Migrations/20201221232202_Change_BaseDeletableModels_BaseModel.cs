namespace TizianaTerenzi.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Change_BaseDeletableModels_BaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductTypes_IsDeleted",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_IsDeleted",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_FragranceGroups_IsDeleted",
                table: "FragranceGroups");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "FragranceGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FragranceGroups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ProductTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Notes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "FragranceGroups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FragranceGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_IsDeleted",
                table: "ProductTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_IsDeleted",
                table: "Notes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FragranceGroups_IsDeleted",
                table: "FragranceGroups",
                column: "IsDeleted");
        }
    }
}
