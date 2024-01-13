#nullable disable

namespace TizianaTerenzi.Orders.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class Add_Columns_With_User_Data_In_Orders_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserLastName",
                table: "Orders",
                newName: "UserTown");

            migrationBuilder.RenameColumn(
                name: "UserFirstName",
                table: "Orders",
                newName: "UserShippingAddress");

            migrationBuilder.AddColumn<byte>(
                name: "CartDiscountCodeValue",
                table: "Orders",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCountry",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserFullName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserPhoneNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserPostalCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartDiscountCodeValue",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserCountry",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserFullName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserPhoneNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserPostalCode",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserTown",
                table: "Orders",
                newName: "UserLastName");

            migrationBuilder.RenameColumn(
                name: "UserShippingAddress",
                table: "Orders",
                newName: "UserFirstName");
        }
    }
}
