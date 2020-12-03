namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Products;

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

        public async Task<bool> EditProductAsync(Product product)
        {
            this.productsRepository.Update(product);
            var result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<ProductsListViewModel> GetAllProductsAsync(int take, int skip = 0)
        {
            var products = await this.productsRepository
                .All()
                .OrderByDescending(p => p.YearOfManufacture)
                .Skip(skip)
                .Take(take)
                .To<ProductInListViewModel>()
                .ToListAsync();

            var viewModel = new ProductsListViewModel()
            {
                Products = products,
            };

            return viewModel;
        }

        public async Task<Product> GetProductByIdAsync(int? id)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Id == id)
                .Include(p => p.ProductType) // TODO: do not use include
                .Include(p => p.FragranceGroup)
                .Include(p => p.Notes)
                .ThenInclude(n => n.Note)
                .SingleOrDefaultAsync();

            return product;
        }

        public async Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(int? id)
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

        public async Task<int> GetProductsCountAsync()
        {
            var count = await this.productsRepository
                .All()
                .CountAsync();

            return count;
        }

        public async Task<int> GetProductTypeIdByProductIdAsync(int? productId)
        {
            var productTypeId = await this.productsRepository
                .All()
                .Where(p => p.Id == productId)
                .Select(p => p.ProductTypeId)
                .SingleOrDefaultAsync();

            return productTypeId;
        }

        public async Task<int> GetFragranceGroupIdByProductIdAsync(int? productId)
        {
            var fragranceGroupId = await this.productsRepository
                .All()
                .Where(p => p.Id == productId)
                .Select(p => p.FragranceGroupId)
                .SingleOrDefaultAsync();

            return fragranceGroupId;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await this.productsRepository
                .All()
                .SingleOrDefaultAsync(p => p.Id == productId);

            this.productsRepository.Delete(product);
            var result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
