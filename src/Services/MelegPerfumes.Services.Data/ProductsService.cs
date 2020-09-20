namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;
    using MelegPerfumes.Web.ViewModels.Products;
    using Microsoft.EntityFrameworkCore;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(IDeletableEntityRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            await this.productsRepository.AddAsync(product);
            int result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task CreateProductsRangeAsync(IEnumerable<Product> products)
        {
            await this.productsRepository.AddRangeAsync(products);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductsListingViewModel>> GetAllProductsAsync()
        {
            var products = await this.productsRepository
                .All()
                .To<ProductsListingViewModel>()
                .ToListAsync();

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Id == id)
                .Include(p => p.ProductType)
                .Include(p => p.FragranceGroup)
                .Include(p => p.Notes)
                .ThenInclude(n => n.Note)
                //.To<T>()
                .SingleOrDefaultAsync();

            return product;
        }
    }
}
