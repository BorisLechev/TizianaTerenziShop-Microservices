namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<IEnumerable<ProductsListingViewModel>> GetAllProductsAsync();

        Task<ProductDetailsViewModel> GetProductByIdAsync(int id);

        Task<bool> CreateProductAsync(Product product);

        Task CreateProductsRangeAsync(IEnumerable<Product> products);
    }
}
