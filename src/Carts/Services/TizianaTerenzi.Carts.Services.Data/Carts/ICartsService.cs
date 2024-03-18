namespace TizianaTerenzi.Carts.Services.Data.Carts
{
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Messages.Products;

    public interface ICartsService
    {
        Task<bool> AddProductInTheCartAsync(ProductAddedInTheCartMessage product);

        Task<bool> DeleteProductInTheCartAsync(string id);

        Task<bool> DeleteProductInAllCartsAsync(ProductInAllCartsDeletedMessage message);

        Task<bool> DeleteAllProductsInTheCartByUserIdAsync(string userId);

        Task<bool> IsThereAnyProductsInTheUsersCartAsync(string userId);

        Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCartByUserIdAsync(string userId);

        Task<string> GetProductInTheCartIdByProductIdAsync(int productId, string userId);

        Task<bool> CheckIfProductExistsInTheUsersCartAsync(string userId, int productId);

        Task<bool> IncreaseQuantityAsync(string cartId);

        Task<bool> ReduceQuantityAsync(string cartId);

        Task<int> GetNumberOfProductsInTheUsersCart(string userId);

        Task Order(ProductsInTheUserCartHaveBeenOrderedInputModel inputModel, string userId);

        Task<bool> EditProductInTheCartAsync(ProductInAllCartsEditedMessage message);
    }
}
