namespace TizianaTerenzi.Products.Web.Gateway.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditProductResponseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public IFormFile Picture { get; set; }

        public IEnumerable<string> NoteIds { get; set; }

        public IEnumerable<SelectListItem> Notes { get; set; }

        public int ProductTypeId { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }

        public int FragranceGroupId { get; set; }

        public IEnumerable<SelectListItem> FragranceGroups { get; set; }

        public int YearOfManufacture { get; set; }
    }
}
