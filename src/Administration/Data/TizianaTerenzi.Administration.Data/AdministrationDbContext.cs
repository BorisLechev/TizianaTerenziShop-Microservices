namespace TizianaTerenzi.Administration.Data
{
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data;

    public class AdministrationDbContext : EventMessageLogDbContext
    {
        public AdministrationDbContext(DbContextOptions<AdministrationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserStatistics> UserStatistics { get; set; }

        public DbSet<OrderStatistics> OrderStatistics { get; set; }

        public DbSet<OrderProductStatistics> OrderProductStatistics { get; set; }

        public DbSet<GeneralDiscount> GeneralDiscounts { get; set; }

        public DbSet<DiscountCodeStatistics> DiscountCodeStatistics { get; set; }

        public DbSet<ContactFormEntry> ContactFormEntries { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Needed for Identity models configuration
            base.OnModelCreating(modelBuilder);
        }
    }
}
