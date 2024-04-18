namespace TizianaTerenzi.Products.Services.Data.ProductTypes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;
    using TizianaTerenzi.Products.Data.Models;

    [TransientRegistration]
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
    }
}
