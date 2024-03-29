namespace TizianaTerenzi.Notifications.Data
{
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data;
    using TizianaTerenzi.Notifications.Data.Models;

    public class NotificationsDbContext : EventMessageLogDbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUserNotification> ApplicationUserNotifications { get; set; }

        public DbSet<ApplicationUserCartNotification> ApplicationUserCartNotifications { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Needed for Identity models configuration
            base.OnModelCreating(modelBuilder);
        }
    }
}
