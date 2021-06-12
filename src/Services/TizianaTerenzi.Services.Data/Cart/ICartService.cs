namespace TizianaTerenzi.Services.Data.Cart
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Orders;

    public interface ICartService
    {
        Task<bool> AddProductInTheCartAsync(Product product, string userId);

        Task<int> DeleteProductInTheCartAsync(string productId);

        Task<int> DeleteAllProductsInTheCartByUserIdAsync(string userId);

        Task<bool> IsThereAnyProductsInTheUsersCartAsync(string userId);

        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCartByUserIdAsync(string userId);

        Task<string> GetProductInTheCartIdByProductIdAsync(int productId);

        Task<bool> CheckIfProductExistsInTheUsersCartAsync(string userId, int productId);

        Task<bool> ReduceQuantityAsync(string productId);

        Task<bool> IncreaseQuantityAsync(string productId);

        Task<bool> CheckoutAsync(string userId);

        Task<bool> SaveShippingDataAsync(ApplicationUser user, ShippingDataInputModel inputModel);
    }
}
