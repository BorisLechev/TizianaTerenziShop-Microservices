namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Products;

    public class ProductSortingsService : IProductSortingsService
    {
        private readonly IRepository<ProductSorting> productSortingsRepository;

        public ProductSortingsService(IRepository<ProductSorting> productSortingsRepository)
        {
            this.productSortingsRepository = productSortingsRepository;
        }

        public async Task<IEnumerable<ProductSortingsViewModel>> GetAllProductSortingsAsync()
        {
            var productSortings = await this.productSortingsRepository
                .AllAsNoTracking()
                .To<ProductSortingsViewModel>()
                .ToListAsync();

            return productSortings;
        }
    }
}
