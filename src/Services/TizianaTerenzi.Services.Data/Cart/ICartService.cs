namespace TizianaTerenzi.Services.Data.Cart
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Orders;

    public interface ICartService
    {
        Task<bool> AddProductInTheCart(Product product, string userId);

        Task<int> DeleteProductInTheCart(string productId);

        Task<int> DeleteAllProductsInTheCartByUserId(string userId);

        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCartByUserId(string userId);

        Task<string> GetProductInTheCartIdByProductIdAsync(int productId);

        Task<bool> CheckIfProductByUserIdExistInTheCartAsync(string userId, int productId);

        Task<bool> ReduceQuantity(string productId);

        Task<bool> IncreaseQuantity(string productId);

        Task<bool> CheckoutAsync(string userId, IEnumerable<ProductsInTheCartViewModel> productsInTheCart);

        Task SaveShippingDataAsync(ApplicationUser user, ShippingDataInputModel inputModel);
    }
}
