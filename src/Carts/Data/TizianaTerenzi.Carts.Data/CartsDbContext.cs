namespace TizianaTerenzi.Carts.Data
{
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Common.Data;

    public class CartsDbContext : EventMessageLogDbContext
    {
        public CartsDbContext(DbContextOptions<CartsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<DiscountCode> DiscountCodes { get; set; }

        public DbSet<Country> Countries { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Needed for Identity models configuration
            base.OnModelCreating(modelBuilder);
        }
    }
}
