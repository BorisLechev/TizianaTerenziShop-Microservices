namespace TizianaTerenzi.WebClient.Services.Carts
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Orders;

    public interface ICartsService
    {
        [Get("/Carts/Index")]
        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheUsersCart();

        [Post("/Carts/IncreaseQuantity")]
        Task<Result> IncreaseQuantity(string productId);

        [Post("/Carts/ReduceQuantity")]
        Task<Result> ReduceQuantity(string productId);

        [Delete("/Carts/DeleteProductInTheCart")]
        Task<Result> DeleteProductInTheCart(string id);
    }
}
