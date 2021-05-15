namespace TizianaTerenzi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddSearchCatalogQuery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT CATALOG SearchCatalog AS DEFAULT;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON [Products]([SearchText]) KEY INDEX PK_Products;",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
