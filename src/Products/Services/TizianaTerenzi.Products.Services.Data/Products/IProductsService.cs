namespace TizianaTerenzi.Products.Services.Data.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Products.Data.Models;
    using TizianaTerenzi.Products.Web.Models.Products;

    public interface IProductsService
    {
        Task<ProductsListViewModel> GetAllProductsAsync(IQueryable<Product> query, string search, ProductSorting sorting, int page, int take, int skip = 0);

        Task<Product> GetProductByIdAsync(int id);

        Task<T> GetProductByIdAsync<T>(int id);

        Task<bool> CreateProductAsync(ProductCreatedMessage message, string pictureUrl);

        Task<bool> EditProductAsync(ProductEditedMessage message, string pictureUrl);

        Task<bool> DeleteProductAsync(int productId);

        Task<bool> UpdateThePricesOfAllProductsAfterTheDiscountIsAppliedAsync(int discountPercent);

        Task<bool> UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync();

        Task<IEnumerable<RelatedProductsViewModel>> GetRandomRelatedProductsAsync(int productId);

        Task AddProductInTheCart(int productId, string userId);
    }
}
