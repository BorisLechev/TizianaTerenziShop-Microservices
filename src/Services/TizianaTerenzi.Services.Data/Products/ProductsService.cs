namespace TizianaTerenzi.Services.Data.Products
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.FragranceGroups;
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        private readonly IFragranceGroupsService fragranceGroupsService;

        private readonly IProductTypesService productTypesService;

        private readonly INotesService notesService;

        private readonly ICloudinaryService cloudinaryService;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository,
            IFragranceGroupsService fragranceGroupsService,
            IProductTypesService productTypesService,
            INotesService notesService,
            ICloudinaryService cloudinaryService)
        {
            this.productsRepository = productsRepository;
            this.fragranceGroupsService = fragranceGroupsService;
            this.productTypesService = productTypesService;
            this.notesService = notesService;
            this.cloudinaryService = cloudinaryService;
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

        public async Task<bool> EditProductAsync(EditProductInputModel inputModel, Product product)
        {
            if (inputModel.Picture != null)
            {
                string pictureUrl = await this.cloudinaryService.UploadPictureAsync(inputModel.Picture, inputModel.Name);
                product.Picture = pictureUrl;
            }

            var notesIds = inputModel.NoteIds.Select(int.Parse);

            await this.notesService.DeleteProductNotesAsync(product.Id);

            var fragranceGroup = await this.fragranceGroupsService.GetFragranceGroupById(inputModel.FragranceGroupId);
            var productType = await this.productTypesService.GetProductTypeById(inputModel.ProductTypeId);

            product.Name = inputModel.Name;
            product.Description = inputModel.Description;
            product.Price = inputModel.Price;
            product.YearOfManufacture = inputModel.YearOfManufacture;
            product.FragranceGroupId = inputModel.FragranceGroupId;
            product.FragranceGroup = fragranceGroup;
            product.ProductTypeId = inputModel.ProductTypeId;
            product.ProductType = productType;
            product.Notes = notesIds.Select(id => new ProductNotes
            {
                NoteId = id,
            })
            .ToList();

            product.SearchText = this.GetSearchText(product.Name, product.Description);

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
                .AllAsNoTracking()
                .Where(p => p.Id == id)
                .Include(p => p.ProductType) // TODO: do not use include
                .Include(p => p.FragranceGroup)
                .Include(p => p.Notes)
                .ThenInclude(n => n.Note)
                .SingleOrDefaultAsync();

            return product;
        }

        public async Task<T> GetProductByIdAsync<T>(int? id)
        {
            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(p => p.Id == id)
                .Include(p => p.ProductType) // TODO: do not use include
                .Include(p => p.FragranceGroup)
                .Include(p => p.Notes)
                .ThenInclude(n => n.Note)
                .To<T>()
                .SingleOrDefaultAsync();

            return product;
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
