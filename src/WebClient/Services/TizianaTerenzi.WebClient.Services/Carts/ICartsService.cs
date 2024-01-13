namespace TizianaTerenzi.WebClient.Services.Carts
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Cart;
    using TizianaTerenzi.WebClient.ViewModels.Orders;

    public interface ICartsService
    {
        [Get("/Carts/Index")]
        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheUsersCart();

        [Post("/Carts/IncreaseQuantity")]
        Task<Result> IncreaseQuantity(string productId);

        [Post("/Carts/ReduceQuantity")]
        Task<Result> ReduceQuantity(string productId);

        [Delete("/Carts/DeleteProduct")]
        Task<Result> DeleteProductInTheCart(string id);

        [Post("/DiscountCodes/Apply")]
        Task<Result> ApplyDiscountCode(string discountName);

        [Delete("/DiscountCodes/Delete")]
        Task<Result> DeleteDiscountCode(string discountName);

        [Post("/Carts/Order")]
        Task<Result<bool>> Order(OrderGatewayModel model);
    }
}
