namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.WebClient.ViewModels.Products;

    public class ProductsController : BaseController
    {
        private readonly TizianaTerenzi.WebClient.Services.Products.IProductsService productsService;

        private readonly IProductsService productsServiceOld;

        private readonly IProductVotesService productVotesService;

        public ProductsController(
            TizianaTerenzi.WebClient.Services.Products.IProductsService productsService,
            IProductsService productsServiceOld,
            IProductVotesService productVotesService)
        {
            this.productsService = productsService;
            this.productsServiceOld = productsServiceOld;
            this.productVotesService = productVotesService;
        }

        public async Task<IActionResult> All(string search, ProductSorting sorting, int page = 1)
        {
            var productsViewModel = await this.productsService.All(search, sorting, page);

            return this.View(productsViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                this.NotFound();
            }

            var productDetailsViewModel = await this.productsServiceOld.GetProductByIdAsync<ProductDetailsViewModel>(id);

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

            var relatedProducts = await this.productsServiceOld.GetRandomRelatedProductsAsync(id);
            productDetailsViewModel.RelatedProducts = relatedProducts;

            return this.View(productDetailsViewModel);
        }
    }
}
