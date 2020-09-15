namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<IEnumerable<ProductsListingViewModel>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);
    }
}
