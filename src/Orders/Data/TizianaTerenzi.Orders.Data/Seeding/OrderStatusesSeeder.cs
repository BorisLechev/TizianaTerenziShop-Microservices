namespace TizianaTerenzi.Orders.Data.Seeding
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Orders.Data.Models;

    public class OrderStatusesSeeder : ISeeder<OrdersDbContext>
    {
        public async Task SeedAsync(OrdersDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.OrderStatuses.AnyAsync())
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
