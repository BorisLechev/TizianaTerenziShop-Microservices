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
            var orderStatuses = new List<string>
            {
                "Pending",
                "Sent",
                "Completed",
            };

            foreach (var orderStatus in orderStatuses)
            {
                if (!dbContext.OrderStatuses.Any(os => os.Name == orderStatus))
                {
                    await dbContext.OrderStatuses.AddAsync(
                        new OrderStatus
                        {
                            Name = orderStatus,
                        });
                }
            }
        }
    }
}
