namespace TizianaTerenzi.Products.Services.Data.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Messages.Products;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Products.Data.Models;
    using TizianaTerenzi.Products.Services.Data.Notes;
    using TizianaTerenzi.Products.Web.Models.Products;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        private readonly INotesService notesService;

        private readonly IBus publisher;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository,
            INotesService notesService,
            IBus publisher)
        {
            this.productsRepository = productsRepository;
            this.notesService = notesService;
            this.publisher = publisher;
        }

        public async Task<bool> CreateProductAsync(ProductCreatedMessage message, string pictureUrl)
        {
            var product = new Product
            {
                Name = message.Name,
                Description = message.Description,
                ProductTypeId = message.ProductTypeId,
                FragranceGroupId = message.FragranceGroupId,
                YearOfManufacture = message.YearOfManufacture,
                Price = message.Price,
                PriceWithGeneralDiscount = message.Price,
                Picture = pictureUrl,
                Notes = message.NoteIds.Select(id => new ProductNote
                {
                    NoteId = id,
                })
                .ToList(),
                SearchText = StringExtensions.GetSearchText(message.Name),
            };

            await this.productsRepository.AddAsync(product);
            int result = await this.productsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditProductAsync(ProductEditedMessage message, string pictureUrl)
        {
            var product = await this.GetProductByIdAsync(message.ProductId);

            await this.notesService.HardDeleteAllProductNotesAsync(product.Id);

            product.Name = message.Name;
            product.Description = message.Description;
            product.Price = message.Price;
            product.PriceWithGeneralDiscount = message.Price;
            product.YearOfManufacture = message.YearOfManufacture;
            product.FragranceGroupId = message.FragranceGroupId;
            product.ProductTypeId = message.ProductTypeId;
            product.Picture = string.IsNullOrWhiteSpace(pictureUrl) ? product.Picture : pictureUrl;
            product.Notes = message.NoteIds.Select(id => new ProductNote
            {
                NoteId = id,
            })
            .ToList();

            product.SearchText = StringExtensions.GetSearchText(product.Name);

            await this.productsRepository.UpdateAsync(product);
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
                .All()
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.PriceWithGeneralDiscount, p => p.Price - (p.Price * discountPercent / 100))
                    .SetProperty(p => p.ModifiedOn, DateTime.UtcNow));

            return affectedRows > 0;
        }

        public async Task<bool> UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync()
        {
            var affectedRows = await this.productsRepository
                                    .All()
                                    .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(p => p.PriceWithGeneralDiscount, p => p.Price)
                                        .SetProperty(p => p.ModifiedOn, DateTime.UtcNow));

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

        public async Task AddProductInTheCart(int productId, string userId)
        {
            var product = await this.productsRepository
                                .AllAsNoTracking()
                                .Where(p => p.Id == productId)
                                .Select(p => new
                                {
                                    Name = p.Name,
                                    Picture = p.Picture,
                                    PriceWithGeneralDiscount = p.PriceWithGeneralDiscount,
                                })
                                .SingleOrDefaultAsync();

            var messageData = new ProductAddedInTheCartMessage
            {
                UserId = userId,
                ProductId = productId,
                ProductName = product.Name,
                ProductPicture = product.Picture,
                Price = product.PriceWithGeneralDiscount,
            };

            var message = new EventMessageLog(messageData);

            await this.productsRepository.CreateEventMessageLog(message);
            await this.productsRepository.SaveChangesAsync();
            await this.productsRepository.MarkEventMessageLogAsPublished(message.Id);

            await this.publisher.Publish(messageData);
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
