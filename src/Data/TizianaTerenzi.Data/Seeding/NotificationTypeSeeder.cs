namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public class NotificationTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.NotificationTypes.Any())
            {
                return;
            }

            var notificationTypes = new List<string>
            {
                "Message",
            };

            var notificationTypeModels = notificationTypes.Select(nt => new NotificationType
            {
                Name = nt,
            });

            await dbContext.NotificationTypes.AddRangeAsync(notificationTypeModels);
        }
    }
}
