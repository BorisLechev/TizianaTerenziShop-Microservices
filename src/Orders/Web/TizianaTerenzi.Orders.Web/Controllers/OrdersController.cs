namespace TizianaTerenzi.Orders.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;
    using TizianaTerenzi.Orders.Services.Data.Orders;
    using TizianaTerenzi.Orders.Web.Models.Orders;

    [Authorize]
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

        [HttpGet]
        [AuthorizeAdministrator]
        public async Task<ActionResult<IEnumerable<OrdersListingViewModel>>> All()
        {
            var orders = await this.ordersService.GetAllOrdersAsync();

            return this.Ok(orders);
        }

        [HttpGet]
        [AuthorizeAdministrator]
        public async Task<ActionResult<IEnumerable<OrdersListingViewModel>>> Pending()
        {
            var orders = await this.ordersService.GetAllPendingOrdersAsync();

            return this.Ok(orders);
        }

        [HttpGet]
        [AuthorizeAdministrator]
        public async Task<ActionResult<IEnumerable<OrdersListingViewModel>>> Processed()
        {
            var orders = await this.ordersService.GetAllProcessedOrdersAsync();

            return this.Ok(orders);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalDataOrdersViewModel>>> GetAllUsersOrdersAndProductsPersonalData()
        {
            var userId = this.User.GetUserId();

            var orderProducts = await this.ordersService.GetAllUsersOrdersAndProductsPersonalData(userId);

            return this.Ok(orderProducts);
        }
    }
}
