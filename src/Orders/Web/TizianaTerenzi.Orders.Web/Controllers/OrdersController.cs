namespace TizianaTerenzi.Orders.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Orders.Services.Data.Orders;
    using TizianaTerenzi.Orders.Web.Models.Orders;

    public class OrdersController : ApiController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersListingViewModel>>> Index()
        {
            var userId = this.User.GetUserId();

            var allOrdersByUser = await this.ordersService.GetAllOrdersByUserIdAsync(userId);

            return this.Ok(allOrdersByUser);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderProductsListingViewModel>>> Products(int orderId)
        {
            var allOrderProductsByUser = await this.ordersService.GetAllOrderProductsByOrderIdAsync(orderId);

            return this.Ok(allOrderProductsByUser);
        }
    }
}
