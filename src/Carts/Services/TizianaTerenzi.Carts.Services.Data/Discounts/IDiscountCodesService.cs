namespace TizianaTerenzi.Carts.Services.Data.Discounts
{
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Common.Messages.Administration;

    public interface IDiscountCodesService
    {
        Task<bool> CheckIfThereIsSuchaDiscountAsync(string discountCodeName);

        Task<DiscountCode> GetDiscountCodeByNameAsync(string discountCodeName);

        Task<bool> ModifyThePricesAfterAppliedDiscountCodeAsync(string discountCodeName, string userId);

        Task<bool> ModifyThePricesAfterDeletedDiscountCodeAsync(string discountName, string userId);

        Task<bool> CreateDiscountCodeAsync(DiscountCodeCreatedMessage inputModel);

        Task<bool> DeleteDiscountCodeAsync(DiscountCodeDeletedMessage message);
    }
}
