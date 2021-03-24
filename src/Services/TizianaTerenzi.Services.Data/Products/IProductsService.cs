namespace TizianaTerenzi.Services.Data.Products
{
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<ProductsListViewModel> GetAllProductsAsync(IQueryable<Product> query, string search, string criteria, int page, int take, int skip = 0);

        Task<Product> GetProductByIdAsync(int id);

        Task<T> GetProductByIdAsync<T>(int id);

        Task<bool> CreateProductAsync(CreateProductInputModel inputModel, string pictureUrl);

        Task<bool> EditProductAsync(EditProductInputModel inputModel, int productId, string pictureUrl);

        Task<bool> DeleteProductAsync(int productId);

        Task<int> UpdateThePricesOfAllProductsAfterTheDiscountIsAppliedAsync(int discountPercent);

        Task<int> UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync();
    }
}
