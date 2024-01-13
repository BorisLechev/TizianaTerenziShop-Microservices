namespace TizianaTerenzi.Orders.Services.Data.Orders
{
    using TizianaTerenzi.Common.Messages.Carts;

    public interface IOrdersService
    {
        Task<bool> OrderAsync(ProductsInTheUserCartHaveBeenOrderedMessage model);
    }
}
