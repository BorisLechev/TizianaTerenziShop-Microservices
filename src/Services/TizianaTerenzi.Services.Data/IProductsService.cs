namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<ProductsWithPaginationListingViewModel> GetAllProductsAsync(int take, int skip = 0);

        Task<Product> GetProductByIdAsync(int? id);

        Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(int? id);

        Task<bool> CreateProductAsync(Product product);

        Task CreateProductsRangeAsync(IEnumerable<Product> products);

        Task<int> GetProductsCountAsync();

        Task<bool> EditProductAsync(Product product);

        Task<int> GetProductTypeIdByProductIdAsync(int? productId);

        Task<int> GetFragranceGroupIdByProductIdAsync(int? productId);
    }
}
