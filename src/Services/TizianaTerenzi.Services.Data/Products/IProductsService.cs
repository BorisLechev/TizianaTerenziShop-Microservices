namespace TizianaTerenzi.Services.Data.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.WebClient.ViewModels.Products;

    public interface IProductsService
    {
        Task<ProductsListViewModel> GetAllProductsAsync(IQueryable<Product> query, string search, ProductSorting sorting, int page, int take, int skip = 0);

        Task<Product> GetProductByIdAsync(int id);

        Task<T> GetProductByIdAsync<T>(int id);

        Task<bool> CreateProductAsync(CreateProductInputModel inputModel, string pictureUrl);

        Task<bool> EditProductAsync(EditProductInputModel inputModel, int productId, string pictureUrl);

        Task<bool> DeleteProductAsync(int productId);

        Task<bool> UpdateThePricesOfAllProductsAfterTheDiscountIsAppliedAsync(int discountPercent);

        Task<bool> UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync();

        Task<IEnumerable<RelatedProductsViewModel>> GetRandomRelatedProductsAsync(int productId);
    }
}
