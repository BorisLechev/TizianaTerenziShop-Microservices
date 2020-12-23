namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Web.ViewModels.Orders;

    [ApiController]
    [Route("administration/api/[controller]")]
    public class ChartsController : AdministrationController
    {
        private readonly IOrdersService ordersService;

        public ChartsController(
            IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task<ActionResult<OrdersChartResponseModel>> Index()
        {
            var orders = await this.ordersService.GetAllOrdersAsync();

            return new OrdersChartResponseModel
            {
                Orders = orders,
            };
        }
    }
}
