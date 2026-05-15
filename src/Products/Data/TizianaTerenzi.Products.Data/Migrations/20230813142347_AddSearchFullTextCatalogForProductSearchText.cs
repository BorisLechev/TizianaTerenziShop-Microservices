#nullable disable

namespace TizianaTerenzi.Products.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddSearchFullTextCatalogForProductSearchText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Full-Text Catalog if not exists
            migrationBuilder.Sql(
                sql: @"IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'SearchCatalog')
                        BEGIN
                            CREATE FULLTEXT CATALOG SearchCatalog AS DEFAULT;
                        END",
                suppressTransaction: true);

            // Create Full-Text Index if not exists
            migrationBuilder.Sql(
                sql: @"IF NOT EXISTS (
                            SELECT * FROM sys.fulltext_indexes fi
                            JOIN sys.tables t ON fi.object_id = t.object_id
                            WHERE t.name = 'Products')
                        BEGIN
                            CREATE FULLTEXT INDEX ON [Products]([SearchText]) KEY INDEX PK_Products;
                        END",
                suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove Full-Text Index, if exists
            migrationBuilder.Sql(
                sql: @"IF EXISTS (
                            SELECT * FROM sys.fulltext_indexes fi
                            JOIN sys.tables t ON fi.object_id = t.object_id
                            WHERE t.name = 'Products')
                        BEGIN
                            DROP FULLTEXT INDEX ON [Products];
                        END",
                suppressTransaction: true);

            // Remove Full-Text Catalog, if exists
            migrationBuilder.Sql(
                sql: @"IF EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'SearchCatalog')
                        BEGIN
                            DROP FULLTEXT CATALOG SearchCatalog;
                        END",
                suppressTransaction: true);
        }
    }
}
