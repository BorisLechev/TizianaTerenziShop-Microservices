namespace TizianaTerenzi.WebClient.ViewModels.GeneralDiscounts
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class GeneralDiscountViewModel
    {
        public byte PercentId { get; set; }

        public IEnumerable<SelectListItem> Percents { get; set; }

        public int IsActive { get; set; }
    }
}
