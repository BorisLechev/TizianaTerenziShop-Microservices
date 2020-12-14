namespace TizianaTerenzi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
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

        public ProductsService()
        {
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            product.SearchText = this.GetSearchText(product.Name, product.Description);

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
            product.SearchText = this.GetSearchText(product.Name, product.Description);

            var result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<ProductsListViewModel> GetAllProductsAsync(IQueryable<Product> query, string search, string criteria, int page, int take, int skip = 0)
        {
            query = criteria switch
            {
                "all-products" => this.GetAllProductsQueryable(query),
                "product-a-z" => this.GetAllProductsOrderedByLetterAscendingQueryable(query),
                "price-ascending" => this.GetAllProductsByPriceAscendingQueryable(query),
                "price-descending" => this.GetAllProductsByPriceDescendingQueryable(query),
                "year-of-release-ascending" => this.GetAllProductsByManufacturedOnAscendingQueryable(query),
                "year-of-release-descending" => this.GetAllProductsByManufacturedOnDescendingQueryable(query),
                _ => this.GetAllProductsQueryable(query),
            };

            var products = await query
                        .Skip(skip)
                        .Take(take)
                        .To<ProductInListViewModel>()
                        .ToListAsync();

            var productsCount = await query.CountAsync(); // without await and CountAsync() ??

            var viewModel = new ProductsListViewModel
            {
                Products = products,
                CurrentPage = page,
                ItemsCount = productsCount,
                ItemsPerPage = take,
                Search = search,
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

        public string GetSearchText(string name, string description)
        {
            // Append title
            var text = name + " " + description;
            text.ToLower();

            // Remove all non-alphanumeric characters
            var regex = new Regex(@"[^\w\d]", RegexOptions.Compiled);
            text = regex.Replace(text, " ");

            // Split words and remove duplicate values
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length > 1).Distinct();

            // Combine all words
            return string.Join(" ", words);
        }

        private IQueryable<Product> GetAllProductsQueryable(IQueryable<Product> query)
        {
            return query
                .OrderByDescending(p => p.YearOfManufacture)
                .ThenByDescending(p => p.Price);
        }

        private IQueryable<Product> GetAllProductsByPriceAscendingQueryable(IQueryable<Product> query)
        {
            return query.OrderBy(p => p.Price);
        }

        private IQueryable<Product> GetAllProductsByPriceDescendingQueryable(IQueryable<Product> query)
        {
            return query.OrderByDescending(p => p.Price);
        }

        private IQueryable<Product> GetAllProductsByManufacturedOnAscendingQueryable(IQueryable<Product> query)
        {
            return query.OrderBy(p => p.YearOfManufacture);
        }

        private IQueryable<Product> GetAllProductsByManufacturedOnDescendingQueryable(IQueryable<Product> query)
        {
            return query.OrderByDescending(p => p.YearOfManufacture);
        }

        private IQueryable<Product> GetAllProductsOrderedByLetterAscendingQueryable(IQueryable<Product> query)
        {
            return query.OrderBy(p => p.Name);
        }
    }
}
