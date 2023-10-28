namespace TizianaTerenzi.Carts.Services.Data.Carts
{
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common.Messages.Products;

    public interface ICartsService
    {
        Task<bool> AddProductInTheCartAsync(ProductAddedInTheCartMessage product);

        Task<bool> DeleteProductInTheCartAsync(string productId);

        Task<bool> DeleteAllProductsInTheCartByUserIdAsync(string userId);

        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCartByUserIdAsync(string userId);

        Task<string> GetProductInTheCartIdByProductIdAsync(int productId, string userId);

        Task<bool> CheckIfProductExistsInTheUsersCartAsync(string userId, int productId);

        Task<bool> IncreaseQuantityAsync(string productId);

        Task<bool> ReduceQuantityAsync(string productId);

        Task<int> GetNumberOfProductsInTheUsersCart(string userId);
    }
}
