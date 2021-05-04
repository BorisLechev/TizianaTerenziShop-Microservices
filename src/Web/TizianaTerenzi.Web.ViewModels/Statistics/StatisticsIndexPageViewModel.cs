namespace TizianaTerenzi.Web.ViewModels.Statistics
{
    using System;
    using System.Collections.Generic;

    public class StatisticsIndexPageViewModel
    {
        public IDictionary<DateTime, int> OrdersFromTheLast10Days { get; set; }

        public IDictionary<DateTime, decimal> SalesValueFromTheLast10Days { get; set; }

        public IDictionary<string, int> NumberOfPurchasesForEachProductForThisMonth { get; set; }
    }
}
