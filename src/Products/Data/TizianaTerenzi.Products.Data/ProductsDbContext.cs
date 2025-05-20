namespace TizianaTerenzi.Products.Data
{
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data;
    using TizianaTerenzi.Products.Data.Models;

    public class ProductsDbContext : EventMessageLogDbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<FragranceGroup> FragranceGroups { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<ProductNote> ProductNotes { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<CommentVote> CommentVotes { get; set; }

        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }

        public DbSet<ProductVote> ProductVotes { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Needed for Identity models configuration
            base.OnModelCreating(modelBuilder);
        }
    }
}
