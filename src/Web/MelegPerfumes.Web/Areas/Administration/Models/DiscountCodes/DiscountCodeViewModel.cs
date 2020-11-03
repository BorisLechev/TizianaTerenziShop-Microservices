namespace MelegPerfumes.Web.Areas.Administration.ViewModels
{
    using System;

    public class DiscountCodeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Discount { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}
