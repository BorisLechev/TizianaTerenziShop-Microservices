namespace TizianaTerenzi.Services.Data.DiscountCodes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.DiscountCodes;

    public interface IDiscountCodesService
    {
        Task<bool> CreateDiscountCodeAsync(CreateDiscountCodeInputModel inputModel);

        Task<bool> DeleteDiscountCodeAsync(int discountId);

        Task<IEnumerable<T>> GetAllDiscountCodesAsync<T>();

        Task<DiscountCode> GetDiscountByNameAsync(string discountName);

        Task<bool> ModifyThePricesAfterAppliedDiscountCodeAsync(DiscountCode discountCode, string userId);

        Task<bool> ModifyThePricesAfterDeletedDiscountCodeAsync(string userId);
    }
}
