namespace TizianaTerenzi.Products.Web.Models.Products
{
    using Microsoft.AspNetCore.Http;

    public class EditProductWithDropdownsResponseModel : PrepareDataForProductCreationAndProductEditingResponseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public IFormFile Picture { get; set; }

        public int YearOfManufacture { get; set; }

        public int ProductTypeId { get; set; }

        public int FragranceGroupId { get; set; }

        public IEnumerable<string> NoteIds { get; set; }
    }
}
