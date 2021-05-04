namespace TizianaTerenzi.Services.Data.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;

        private readonly IDeletableEntityRepository<OrderProduct> orderProductsRepository;

        public StatisticsService(
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<OrderProduct> orderProductsRepository)
        {
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
        }

        public async Task<IDictionary<DateTime, int>> GetAllOrdersForTheLast10DaysAsync()
        {
            var result = this.CreateEmptyDictionaryWithDateTimeIntFor10Days();

            var orders = await this.ordersRepository
                .AllAsNoTracking()
                .Where(o => o.CreatedOn > DateTime.UtcNow.Date.AddDays(-10))
                .GroupBy(o => o.CreatedOn.Date)
                .Select(o => new
                {
                    Day = o.Key,
                    Value = o.Count(),
                })
                .ToDictionaryAsync(x => x.Day, x => x.Value);

            foreach (var order in orders)
            {
                if (result.ContainsKey(order.Key.Date))
                {
                    result[order.Key.Date] = order.Value;
                }
            }

            return result;
        }

        public async Task<IDictionary<DateTime, decimal>> GetTheValueOfAllSalesForTheLast10DaysAsync()
        {
            var result = this.CreateEmptyDictionaryWithDateTimeDecimalFor10Days();

            var orderProducts = await this.orderProductsRepository
                .AllAsNoTracking()
                .Where(o => o.CreatedOn > DateTime.UtcNow.Date.AddDays(-10))
                .GroupBy(o => o.CreatedOn.Date)
                .Select(o => new
                {
                    Day = o.Key,
                    Value = o.Sum(p => p.Price),
                })
                .ToDictionaryAsync(o => o.Day, o => o.Value);

            foreach (var product in orderProducts)
            {
                if (result.ContainsKey(product.Key))
                {
                    result[product.Key.Date] = product.Value;
                }
            }

            return result;
        }

        public async Task<IDictionary<string, int>> GetNumberOfPurchasesForEachProductForThisMonthAsync()
        {
            var orderProducts = await this.orderProductsRepository
                .AllAsNoTracking()
                .Where(op => op.CreatedOn.Month == DateTime.UtcNow.Month)
                .GroupBy(op => op.Product.Name)
                .Select(p => new
                {
                    Product = p.Key,
                    Value = p.Sum(x => x.Quantity),
                })
                .ToDictionaryAsync(op => op.Product, op => op.Value);

            return orderProducts;
        }

        private IDictionary<DateTime, int> CreateEmptyDictionaryWithDateTimeIntFor10Days()
        {
            var result = new Dictionary<DateTime, int>();

            var startDate = DateTime.UtcNow.Date.AddDays(-9);

            for (DateTime i = startDate; i <= DateTime.UtcNow.Date; i = i.AddDays(1))
            {
                result.Add(i.Date, 0);
            }

            return result;
        }

        private IDictionary<DateTime, decimal> CreateEmptyDictionaryWithDateTimeDecimalFor10Days()
        {
            var result = new Dictionary<DateTime, decimal>();

            var startDate = DateTime.UtcNow.Date.AddDays(-9);

            for (DateTime i = startDate; i <= DateTime.UtcNow.Date; i = i.AddDays(1))
            {
                result.Add(i.Date, 0);
            }

            return result;
        }
    }
}
