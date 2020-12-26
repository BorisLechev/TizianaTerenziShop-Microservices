namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public class OrderStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.OrderStatuses.Any())
            {
                return;
            }

            var orderStatuses = new List<string>
            {
                "Pending",
                "Sent",
                "Completed",
            };

            var orderStatusModels = orderStatuses.Select(os => new OrderStatus { Name = os });

            await dbContext.OrderStatuses.AddRangeAsync(orderStatusModels);
        }
    }
}
