namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ProductTypesService : IProductTypesService
    {
        private readonly IDeletableEntityRepository<ProductType> productTypesRepository;

        public ProductTypesService(
            IDeletableEntityRepository<ProductType> productTypesRepository)
        {
            this.productTypesRepository = productTypesRepository;
        }

        public async Task<bool> CreateProductTypeAsync(ProductType productType)
        {
            await this.productTypesRepository.AddAsync(productType);
            int result = await this.productTypesRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task CreateProductTypesRangeAsync(IEnumerable<ProductType> productTypes)
        {
            await this.productTypesRepository.AddRangeAsync(productTypes);
            await this.productTypesRepository.SaveChangesAsync();
        }

        public async Task<ProductType> FindByNameProductType(string productTypeName)
        {
            var productType = await this.productTypesRepository
                .All()
                .SingleOrDefaultAsync(pt => pt.Name == productTypeName);

            return productType;
        }

        public async Task<IEnumerable<ProductType>> GetAllProductTypes()
        {
            var productTypes = await this.productTypesRepository
                .All()
                .ToListAsync();

            return productTypes;
        }
    }
}
