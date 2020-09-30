namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Web.ViewModels.Orders;

    public interface ICartService
    {
        Task<bool> AddProductInTheCart(ProductInTheCart productInTheCart);

        Task<bool> DeleteProductInTheCart(string orderId);

        Task<bool> DeleteAllProductsInTheCartByUserId(string userId);

        Task<IEnumerable<OrdersCartViewModel>> GetAllProductsInTheCartByUserId(string userId);

        Task<OrdersCartViewModel> GetProductById(int productId);

        Task<bool> ReduceQuantity(string orderId);

        Task<bool> IncreaseQuantity(string orderId);
    }
}
