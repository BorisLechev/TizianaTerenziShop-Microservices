namespace TizianaTerenzi.Carts.Services.Data.Discounts
{
    using TizianaTerenzi.Carts.Data.Models;

    public interface IDiscountCodesService
    {
        Task<bool> CheckIfThereIsSuchaDiscountAsync(string discountCodeName);

        Task<DiscountCode> GetDiscountCodeByNameAsync(string discountCodeName);

        Task<bool> ModifyThePricesAfterAppliedDiscountCodeAsync(string discountCodeName, string userId);

        Task<bool> ModifyThePricesAfterDeletedDiscountCodeAsync(string discountName, string userId);
    }
}
