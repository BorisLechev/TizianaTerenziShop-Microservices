namespace TizianaTerenzi.Services.Data.Discounts
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface IGeneralDiscountsService
    {
        Task<GeneralDiscount> GetGeneralDiscountAsync();

        Task<T> GetGeneralDiscountAsync<T>();

        Task<bool> ApplyDiscountToAllProductsAsync(byte percent);

        Task<bool> DisableDiscountToAllProductsAsync();
    }
}
