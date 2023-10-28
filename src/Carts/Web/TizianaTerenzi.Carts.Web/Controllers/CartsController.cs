namespace TizianaTerenzi.Carts.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;

    [Authorize]
    public class CartsController : ApiController
    {
        private readonly ICartsService cartsService;

        public CartsController(ICartsService cartsService)
        {
            this.cartsService = cartsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsInTheCartViewModel>>> Index()
        {
            var userId = this.User.GetUserId();

            var productsInTheCart = await this.cartsService.GetAllProductsInTheCartByUserIdAsync(userId);

            return this.Ok(productsInTheCart);
        }

        [HttpGet]
        public async Task<ActionResult<int>> GetNumberOfProductsInTheUsersCart()
        {
            var userId = this.User.GetUserId();

            int numberOfProductsInTheUsersCart = await this.cartsService.GetNumberOfProductsInTheUsersCart(userId);

            return this.Ok(numberOfProductsInTheUsersCart);
        }

        [HttpPost]
        public async Task<ActionResult<Result>> IncreaseQuantity(string productId)
        {
            bool result = await this.cartsService.IncreaseQuantityAsync(productId);

            if (result)
            {
                return Result.Success();
            }
            else
            {
                return Result.Failure(NotificationMessages.SomethingWentWrong);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Result>> ReduceQuantity(string productId)
        {
            bool result = await this.cartsService.ReduceQuantityAsync(productId);

            if (result)
            {
                return Result.Success();
            }
            else
            {
                return Result.Failure(NotificationMessages.SomethingWentWrong);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Result>> DeleteProduct(string id)
        {
            bool result = await this.cartsService.DeleteProductInTheCartAsync(id);

            if (!result)
            {
                Result.Failure(NotificationMessages.CannotDeleteThisProductInTheCartError);
            }

            return Result.Success();
        }
    }
}
