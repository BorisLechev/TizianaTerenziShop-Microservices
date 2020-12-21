namespace TizianaTerenzi.Services.Data.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class ProductTypesService : IProductTypesService
    {
        private readonly IRepository<ProductType> productTypesRepository;

        public ProductTypesService(
            IRepository<ProductType> productTypesRepository)
        {
            this.productTypesRepository = productTypesRepository;
        }

        public async Task<bool> CreateProductTypeAsync(ProductType productType)
        {
            await this.productTypesRepository.AddAsync(productType);
            int result = await this.productTypesRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllProductTypesAsync()
        {
            var productTypes = await this.productTypesRepository
                .AllAsNoTracking()
                .OrderBy(pt => pt.Name)
                .Select(pt => new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name,
                })
                .ToListAsync();

            return productTypes;
        }

        public async Task<ProductType> GetProductTypeById(int id)
        {
            var productType = await this.productTypesRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(pt => pt.Id == id);

            return productType;
        }
    }
}
