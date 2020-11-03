namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Web.ViewModels.Orders;

    public interface ICartService
    {
        Task<bool> AddProductInTheCart(ProductInTheCart productInTheCart);

        Task<bool> DeleteProductInTheCart(string productId);

        Task<bool> DeleteAllProductsInTheCartByUserId(string userId);

        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCartByUserId(string userId);

        Task<ProductsInTheCartViewModel> GetProductById(int productId);

        Task<bool> CheckIfProductByUserIdExistInTheCartAsync(string userId, int productId);

        Task<bool> ReduceQuantity(string productId);

        Task<bool> IncreaseQuantity(string productId);

        Task<Order> CheckOutAsync(string userId, ICollection<OrderProduct> orderProducts, int? discountCodeId);
    }
}
