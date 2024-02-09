namespace TizianaTerenzi.Administration.Services.Data.GeneralDiscounts
{
    using TizianaTerenzi.Administration.Data.Models;

    public interface IGeneralDiscountsService
    {
        Task<GeneralDiscount> GetGeneralDiscountAsync();

        Task<T> GetGeneralDiscountAsync<T>();

        Task<bool> ApplyDiscountToAllProductsAsync(byte percent);

        Task<bool> DisableDiscountToAllProductsAsync();
    }
}
