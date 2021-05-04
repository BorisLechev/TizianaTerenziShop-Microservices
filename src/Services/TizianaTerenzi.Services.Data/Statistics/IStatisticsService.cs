namespace TizianaTerenzi.Services.Data.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStatisticsService
    {
        Task<IDictionary<DateTime, int>> GetAllOrdersForTheLast10DaysAsync();

        Task<IDictionary<DateTime, decimal>> GetTheValueOfAllSalesForTheLast10DaysAsync();

        Task<IDictionary<string, int>> GetNumberOfPurchasesForEachProductForTheCurrentMonthAsync();

        Task<decimal> GetTotalRevenueForTheCurrentMonthAsync();

        Task<int> GetNumberOfOrdersForTheCurrentMonthAsync();

        Task<int> GetNumberOfRegisteredUsersAsync();
    }
}
