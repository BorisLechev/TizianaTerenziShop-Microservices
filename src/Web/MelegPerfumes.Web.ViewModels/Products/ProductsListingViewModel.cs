namespace MelegPerfumes.Web.ViewModels.Products
{
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class ProductsListingViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public int YearOfManufacture { get; set; }
    }
}
