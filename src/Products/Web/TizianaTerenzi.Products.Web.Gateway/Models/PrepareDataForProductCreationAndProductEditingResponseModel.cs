namespace TizianaTerenzi.Products.Web.Gateway.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PrepareDataForProductCreationAndProductEditingResponseModel
    {
        public IEnumerable<SelectListItem> Notes { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }

        public IEnumerable<SelectListItem> FragranceGroups { get; set; }
    }
}
