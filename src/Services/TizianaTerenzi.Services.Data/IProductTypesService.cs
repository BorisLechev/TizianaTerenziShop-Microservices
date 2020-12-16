namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Data.Models;

    public interface IProductTypesService
    {
        Task<bool> CreateProductTypeAsync(ProductType productType);

        Task<IEnumerable<SelectListItem>> GetAllProductTypesAsync();
    }
}
