namespace TizianaTerenzi.Services.Data.Discounts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.WebClient.ViewModels.DiscountCodes;

    public interface IDiscountCodesService
    {
        Task<bool> CreateDiscountCodeAsync(CreateDiscountCodeInputModel inputModel);

        Task<bool> DeleteDiscountCodeAsync(int discountId);

        Task<IEnumerable<T>> GetAllDiscountCodesAsync<T>();

        Task<bool> CheckIfThereIsSuchaDiscountAsync(string discountCodeName);

        Task<DiscountCode> GetDiscountCodeByNameAsync(string discountCodeName);

        Task<bool> ModifyThePricesAfterAppliedDiscountCodeAsync(string discountCodeName, string userId);

        Task<bool> ModifyThePricesAfterDeletedDiscountCodeAsync(string userId);
    }
}
