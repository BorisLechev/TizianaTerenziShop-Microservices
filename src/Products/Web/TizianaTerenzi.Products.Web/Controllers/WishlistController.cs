namespace TizianaTerenzi.Products.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Services.Data.Wishlist;
    using TizianaTerenzi.Products.Web.Models.Wishlist;

    [Authorize]
    public class WishlistController : ApiController
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(
            IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        public async Task<IEnumerable<WishlistViewModel>> Index()
        {
            var userId = this.User.GetUserId();

            var wishlistViewModel = await this.wishlistService.GetAllProductsFromUsersWishlistAsync<WishlistViewModel>(userId);

            return wishlistViewModel;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Add(int productId)
        {
            if (productId <= 0)
            {
                return Result.Failure("Not Found");
            }

            var userId = this.User.GetUserId();

            var isProductAdded = await this.wishlistService.HasTheProductAlreadyAddedToTheWishlistAsync(productId, userId);

            if (isProductAdded)
            {
                return Result.Failure(NotificationMessages.TheProductHasAlreadyBeenAddedToTheWishlist);
            }

            var result = await this.wishlistService.AddProductToTheWishlistAsync(productId, userId);

            if (!result)
            {
                return Result.Failure(NotificationMessages.CannotAddProductToTheWishlist);
            }

            return Result.Success(NotificationMessages.AddProductToTheWishlistSuccessfully);
        }

        [HttpDelete]
        public async Task<ActionResult<Result>> Delete(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();

            var result = await this.wishlistService.DeleteProductFromTheWishlistAsync(productId, userId);

            if (!result)
            {
                return Result.Failure(NotificationMessages.CannotDeleteProductFromTheWishlist);
            }

            return Result.Success(NotificationMessages.DeleteProductFromTheWishlistSuccessfully);
        }

        public async Task<ActionResult<IEnumerable<ProductsFromUsersWishlistPersonalDataResponseModel>>> GetAllProductsFromUsersWishlistPersonalData()
        {
            var userId = this.User.GetUserId();

            var wishlistViewModel = await this.wishlistService.GetAllProductsFromUsersWishlistAsync<ProductsFromUsersWishlistPersonalDataResponseModel>(userId);

            return this.Ok(wishlistViewModel);
        }
    }
}
