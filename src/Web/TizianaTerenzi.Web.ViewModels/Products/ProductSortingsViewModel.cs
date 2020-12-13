namespace TizianaTerenzi.Web.ViewModels.Products
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class ProductSortingsViewModel : IMapFrom<ProductSorting>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
