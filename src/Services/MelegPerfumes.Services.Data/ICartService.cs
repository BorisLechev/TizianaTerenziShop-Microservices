namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Services.Models;
    using MelegPerfumes.Web.ViewModels.Orders;

    public interface ICartService
    {
        Task<bool> AddProductInTheCart(ProductInTheCartServiceModel productInTheCartServiceModel);

        Task<bool> DeleteProductInTheCart(string orderId);

        Task<IEnumerable<OrdersCartViewModel>> GetAllProductsInTheCartByUserId(string userId);

        Task<bool> ReduceQuantity(string orderId);

        Task<bool> IncreaseQuantity(string orderId);
    }
}
