namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Wishlist;
    using TizianaTerenzi.WebClient.Infrastructure.Extensions;

    [Authorize]
    public class WishlistController : BaseController
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(
            IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.GetUserId();

            var wishlistViewModel = await this.wishlistService.GetAllProductsFromUsersWishlistAsync(userId);

            return this.View(wishlistViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();

            var isProductAdded = await this.wishlistService.HasTheProductAlreadyAddedToTheWishlistAsync(productId, userId);

            if (isProductAdded)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var result = await this.wishlistService.AddProductToTheWishlistAsync(productId, userId);

            if (result == false)
            {
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
