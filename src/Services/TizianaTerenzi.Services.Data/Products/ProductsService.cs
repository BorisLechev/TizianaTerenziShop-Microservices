namespace TizianaTerenzi.Services.Data.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.WebClient.ViewModels.Products;
    using Z.EntityFramework.Plus;

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
                PriceWithGeneralDiscount = inputModel.Price,
                Picture = pictureUrl,
                Notes = notesIds.Select(id => new ProductNote
                {
                    NoteId = id,
                })
                .ToList(),
                SearchText = StringExtensions.GetSearchText(inputModel.Name),
            };

            await this.productsRepository.AddAsync(product);
            int result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditProductAsync(EditProductInputModel inputModel, int productId, string pictureUrl)
        {
            var product = await this.GetProductByIdAsync(productId);

            var notesIds = inputModel.NoteIds.Select(int.Parse);

            await this.notesService.HardDeleteAllProductNotesAsync(product.Id);

            product.Name = inputModel.Name;
            product.Description = inputModel.Description;
            product.Price = inputModel.Price;
            product.PriceWithGeneralDiscount = inputModel.Price;
            product.YearOfManufacture = inputModel.YearOfManufacture;
            product.FragranceGroupId = inputModel.FragranceGroupId;
            product.ProductTypeId = inputModel.ProductTypeId;
            product.Picture = string.IsNullOrEmpty(pictureUrl) ? product.Picture : pictureUrl;
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

        public async Task<ProductsListViewModel> GetAllProductsAsync(IQueryable<Product> query, string search, ProductSorting sorting, int page, int take, int skip = 0)
        {
            query = sorting switch
            {
                ProductSorting.Default => this.GetAllProductsQueryable(query),
                ProductSorting.Product_A_Z => this.GetAllProductsOrderedByLetterAscendingQueryable(query),
                ProductSorting.PriceAscending => this.GetAllProductsByPriceAscendingQueryable(query),
                ProductSorting.PriceDescending => this.GetAllProductsByPriceDescendingQueryable(query),
                ProductSorting.YearOfReleaseAscending => this.GetAllProductsByManufacturedOnAscendingQueryable(query),
                ProductSorting.YearOfReleaseDescending => this.GetAllProductsByManufacturedOnDescendingQueryable(query),
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
                Sorting = sorting,
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
            var affectedRows = await this.productsRepository
                .AllAsNoTracking()
                .UpdateAsync(p => new Product
                {
                    PriceWithGeneralDiscount = p.Price - (p.Price * discountPercent / 100),
                    ModifiedOn = DateTime.UtcNow,
                });

            return affectedRows > 0;
        }

        public async Task<bool> UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync()
        {
            var affectedRows = await this.productsRepository
                .AllAsNoTracking()
                .UpdateAsync(p => new Product
                {
                    PriceWithGeneralDiscount = p.Price,
                    ModifiedOn = DateTime.UtcNow,
                });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<RelatedProductsViewModel>> GetRandomRelatedProductsAsync(int productId)
        {
            var randomProducts = await this.productsRepository
                .AllAsNoTracking()
                .Where(p => p.Id != productId)
                .OrderBy(p => Guid.NewGuid())
                .Take(3)
                .To<RelatedProductsViewModel>()
                .ToListAsync();

            return randomProducts;
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
