namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Products;
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

        public async Task<ProductDetailsViewModel> GetProductByIdAsync(int id)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Id == id)
                .Include(p => p.ProductType) // TODO: do not use include
                .Include(p => p.FragranceGroup)
                .Include(p => p.Notes)
                .ThenInclude(n => n.Note)
                .To<ProductDetailsViewModel>()
                .SingleOrDefaultAsync();

            return product;
        }
    }
}
