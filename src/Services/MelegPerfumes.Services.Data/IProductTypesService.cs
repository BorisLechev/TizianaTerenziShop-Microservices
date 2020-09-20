namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;

    public interface IProductTypesService
    {
        Task<bool> CreateProductTypeAsync(ProductType productType);

        Task CreateProductTypesRangeAsync(IEnumerable<ProductType> productTypes);

        Task<IEnumerable<ProductType>> GetAllProductTypes();

        Task<ProductType> FindByNameProductType(string productTypeName);
    }
}
