namespace TizianaTerenzi.Carts.Services.Data.Carts
{
    using TizianaTerenzi.Carts.Web.Models.Carts;

    public interface ICartsService
    {
        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCartByUserIdAsync(string userId);

        Task<bool> IncreaseQuantityAsync(string productId);

        Task<bool> ReduceQuantityAsync(string productId);

        Task<bool> DeleteProductInTheCartAsync(string productId);
    }
}
