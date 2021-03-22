namespace TizianaTerenzi.Services.Data.Products
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        private readonly INotesService notesService;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository,
            INotesService notesService)
        {
            this.productsRepository = productsRepository;
            this.notesService = notesService;
        }

        public ProductsService()
        {
        }

        public async Task<bool> CreateProductAsync(CreateProductInputModel inputModel, string pictureUrl)
        {
            var notesIds = inputModel.NoteIds.Select(int.Parse);

            var product = new Product
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                ProductTypeId = inputModel.ProductTypeId,
                FragranceGroupId = inputModel.FragranceGroupId,
                YearOfManufacture = inputModel.YearOfManufacture,
                Price = inputModel.Price,
                PriceWithDiscount = inputModel.Price,
                Picture = pictureUrl,
                Notes = notesIds.Select(id => new ProductNote
                {
                    NoteId = id,
                })
                .ToList(),
            };

            product.SearchText = StringExtensions.GetSearchText(product.Name);

            await this.productsRepository.AddAsync(product);
            int result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditProductAsync(EditProductInputModel inputModel, int productId, string pictureUrl)
        {
            var product = await this.GetProductByIdAsync(productId);

            var notesIds = inputModel.NoteIds.Select(int.Parse);

            await this.notesService.DeleteAllProductNotesAsync(product.Id);

            product.Name = inputModel.Name;
            product.Description = inputModel.Description;
            product.Price = inputModel.Price;
            product.PriceWithDiscount = inputModel.Price;
            product.YearOfManufacture = inputModel.YearOfManufacture;
            product.FragranceGroupId = inputModel.FragranceGroupId;
            product.ProductTypeId = inputModel.ProductTypeId;
            product.Picture = pictureUrl;
            product.Notes = notesIds.Select(id => new ProductNote
            {
                NoteId = id,
            })
            .ToList();

            product.SearchText = StringExtensions.GetSearchText(product.Name);

            this.productsRepository.Update(product);
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

            var productsCount = await query.CountAsync();

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

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync();

            return product;
        }

        public async Task<T> GetProductByIdAsync<T>(int id)
        {
            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(p => p.Id == id)
                .To<T>()
                .SingleOrDefaultAsync();

            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await this.productsRepository
                .All()
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return false;
            }

            this.productsRepository.Delete(product);
            var result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateThePricesOfAllProductsAfterTheDiscountIsAppliedAsync(int discountPercent)
        {
            var products = await this.productsRepository
                .All()
                .ToListAsync();

            foreach (var product in products)
            {
                product.PriceWithDiscount -= product.Price * discountPercent / 100;
            }

            var result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync()
        {
            var products = await this.productsRepository
                .All()
                .ToListAsync();

            foreach (var product in products)
            {
                product.PriceWithDiscount = product.Price;
            }

            var result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
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
