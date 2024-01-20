#nullable disable

namespace TizianaTerenzi.Administration.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class Initial_Administration_Entities_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProductStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStatisticsId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProductStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProductStatistics_OrderStatistics_OrderStatisticsId",
                        column: x => x.OrderStatisticsId,
                        principalTable: "OrderStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductStatistics_IsDeleted",
                table: "OrderProductStatistics",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductStatistics_OrderStatisticsId",
                table: "OrderProductStatistics",
                column: "OrderStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductStatistics_ProductName",
                table: "OrderProductStatistics",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatistics_IsDeleted",
                table: "OrderStatistics",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatistics_IsDeleted",
                table: "UserStatistics",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatistics_RoleName",
                table: "UserStatistics",
                column: "RoleName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProductStatistics");

            migrationBuilder.DropTable(
                name: "UserStatistics");

            migrationBuilder.DropTable(
                name: "OrderStatistics");
        }
    }
}
