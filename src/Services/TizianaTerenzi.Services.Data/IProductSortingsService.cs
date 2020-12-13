namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.Products;

    public interface IProductSortingsService
    {
        Task<IEnumerable<ProductSortingsViewModel>> GetAllProductSortingsAsync();
    }
}
