namespace TizianaTerenzi.WebClient.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Web.ViewModels.Products;

    public class ProductsController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly IProductsService productsService;

        private readonly IProductVotesService productVotesService;

        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsController(
            IDeletableEntityRepository<Product> productsRepository,
            IProductsService productsService,
            IProductVotesService productVotesService)
        {
            this.productsService = productsService;
            this.productVotesService = productVotesService;
            this.productsRepository = productsRepository;
        }

        public async Task<IActionResult> All(string search, ProductSorting sorting, int page = 1)
        {
            page = Math.Max(1, page);
            var skip = (page - 1) * ItemsPerPage;

            var query = this.productsRepository.AllAsNoTracking();
            var words = search?
                        .Split(' ')
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 2)
                        .ToList();

            if (words != null)
            {
                foreach (var word in words)
                {
                    query = query.Where(c => EF.Functions.FreeText(c.SearchText, word));
                }
            }

            var productsViewModel = await this.productsService.GetAllProductsAsync(query, search, sorting, page, ItemsPerPage, skip);

            return this.View(productsViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                this.NotFound();
            }

            var productDetailsViewModel = await this.productsService.GetProductByIdAsync<ProductDetailsViewModel>(id);

            if (productDetailsViewModel == null)
            {
                return this.NotFound();
            }

            var groupedProductVotes = await this.productVotesService.GetNumberOfVotesForEachValueAsync(id);
            var numberOfVoters = groupedProductVotes != null
                                ? groupedProductVotes.CountOfVotes
                                : 0;
            var averageVotes = groupedProductVotes != null
                                ? groupedProductVotes.AverageVotes
                                : 0;

            var countOfVotesWithValueFive = groupedProductVotes?.GroupVotesWithValue5;
            var countOfVotesWithValueFour = groupedProductVotes?.GroupVotesWithValue4;
            var countOfVotesWithValueThree = groupedProductVotes?.GroupVotesWithValue3;
            var countOfVotesWithValueTwo = groupedProductVotes?.GroupVotesWithValue2;
            var countOfVotesWithValueOne = groupedProductVotes?.GroupVotesWithValue1;

            productDetailsViewModel.ShareOfVotesWithValueOfFive =
                countOfVotesWithValueFive.HasValue && numberOfVoters > 0
                ? (double)countOfVotesWithValueFive / numberOfVoters * 100
                : 0;
            productDetailsViewModel.ShareOfVotesWithValueOfFour =
                countOfVotesWithValueFour.HasValue && numberOfVoters > 0
                ? (double)countOfVotesWithValueFour / numberOfVoters * 100
                : 0;
            productDetailsViewModel.ShareOfVotesWithValueOfThree =
                countOfVotesWithValueThree.HasValue && numberOfVoters > 0
                ? (double)countOfVotesWithValueThree / numberOfVoters * 100
                : 0;
            productDetailsViewModel.ShareOfVotesWithValueOfTwo =
                countOfVotesWithValueTwo.HasValue && numberOfVoters > 0
                ? (double)countOfVotesWithValueTwo / numberOfVoters * 100
                : 0;
            productDetailsViewModel.ShareOfVotesWithValueOfOne =
                countOfVotesWithValueOne.HasValue && numberOfVoters > 0
                ? (double)countOfVotesWithValueOne / numberOfVoters * 100
                : 0;
            productDetailsViewModel.AverageVote = averageVotes;
            productDetailsViewModel.NumberOfVoters = numberOfVoters;

            var relatedProducts = await this.productsService.GetRandomRelatedProductsAsync(id);
            productDetailsViewModel.RelatedProducts = relatedProducts;

            return this.View(productDetailsViewModel);
        }
    }
}
