namespace TizianaTerenzi.Products.Services.Data.ProductTypes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Products.Data.Models;

    public interface IProductTypesService
    {
        Task<bool> CreateProductTypeAsync(ProductType productType);

        Task<IEnumerable<SelectListItem>> GetAllProductTypesAsync();
    }
}
