namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Wishlist;
    using TizianaTerenzi.WebClient.Infrastructure.Extensions;
    using TizianaTerenzi.WebClient.Services.Products;

    [Authorize]
    public class WishlistController : BaseController
    {
        private readonly IWishlistService wishlistService;
        private readonly IProductsService productsService;

        public WishlistController(
            IWishlistService wishlistService,
            IProductsService productsService)
        {
            this.wishlistService = wishlistService;
            this.productsService = productsService;
        }

        public async Task<IActionResult> Index()
        {
            var wishlistViewModel = await this.productsService.GetAllProductsFromUsersWishlist();

            return this.View(wishlistViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var result = await this.productsService.AddProductToTheWishlist(productId);

            if (!result.Succeeded)
            {
                if (result.Errors.Contains(NotificationMessages.TheProductHasAlreadyBeenAddedToTheWishlist))
                {
                    this.Error(NotificationMessages.TheProductHasAlreadyBeenAddedToTheWishlist);

                    return this.RedirectToAction(nameof(this.Index));
                }

                this.Error(NotificationMessages.CannotAddProductToTheWishlist);

                return this.LocalRedirect("/products/all");
            }

            this.Success(NotificationMessages.AddProductToTheWishlistSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();

            var result = await this.wishlistService.DeleteProductFromTheWishlistAsync(productId, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.CannotDeleteProductFromTheWishlist);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.DeleteProductFromTheWishlistSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
