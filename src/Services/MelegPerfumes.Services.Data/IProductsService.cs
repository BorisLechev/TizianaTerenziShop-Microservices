namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<IEnumerable<ProductsListingViewModel>> GetAllProductsAsync();

        Task<T> GetProductByIdAsync<T>(int id);
    }
}
