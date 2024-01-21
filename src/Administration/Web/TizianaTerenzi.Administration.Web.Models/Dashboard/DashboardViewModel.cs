namespace TizianaTerenzi.Administration.Web.Models.Dashboard
{
    using System;
    using System.Collections.Generic;

    public class DashboardViewModel
    {
        public int TotalUsersCount { get; set; }

        public int TotalUsersInAdminRoleCount { get; set; }

        public int TotalBannedUsersCount { get; set; }

        public double BannedUsersPercentage => (double)this.TotalBannedUsersCount / this.TotalUsersCount * 100;

        public int TotalOrderedProductsCountForTheCurrentMonth { get; set; }

        public int TotalOrdersCountForTheCurrentMonth { get; set; }

        public decimal TotalRevenueForTheCurrentMonth { get; set; }

        public decimal TotalRevenueForTheCurrentYear { get; set; }

        public IDictionary<DateTime, decimal> SalesValueFromTheLast10Days { get; set; }

        public IEnumerable<GroupByViewModel<string, int>> NumberOfPurchasesForEachProductForThisMonth { get; set; }
    }
}
