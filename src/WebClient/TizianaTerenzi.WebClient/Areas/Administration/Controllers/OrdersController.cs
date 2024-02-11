namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.Services.Orders;

    public class OrdersController : AdministrationController
    {
        private readonly IOrdersService ordersService;
        private readonly IAdministrationService administrationService;

        public OrdersController(
            IOrdersService ordersService,
            IAdministrationService administrationService)
        {
            this.ordersService = ordersService;
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await this.ordersService.GetAllOrdersAsync();

            return this.View(orders);
        }

        public async Task<IActionResult> Pending()
        {
            var orders = await this.ordersService.GetAllPendingOrdersAsync();

            return this.View(orders);
        }

        public async Task<IActionResult> Processed()
        {
            var orders = await this.ordersService.GetAllProcessedOrdersAsync();

            return this.View(orders);
        }

        public async Task<IActionResult> Products(int id)
        {
            var orderProducts = await this.ordersService.GetAllOrderProductsByOrderIdAsync(id);

            return this.View(orderProducts);
        }

        public async Task<IActionResult> Process(int orderId)
        {
            var result = await this.administrationService.ProcessOrderAsync(orderId);

            if (!result.Succeeded)
            {
                this.Error(NotificationMessages.ProcessOrderError);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.ProcessOrderSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
