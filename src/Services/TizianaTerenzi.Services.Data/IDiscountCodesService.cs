namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface IDiscountCodesService
    {
        Task<bool> CreateDiscountCodeAsync(DiscountCode discountCode);

        Task<bool> DeleteDiscountCodeAsync(int discountId);

        Task<IEnumerable<DiscountCode>> GetAllDiscountCodesAsync();

        Task<DiscountCode> GetDiscountByNameAsync(string discountName);

        Task<bool> ModifyThePricesAfterAppliedDiscountCodeAsync(DiscountCode discountCode, string userId);

        Task<bool> ModifyThePricesAfterDeletedDiscountCodeAsync(DiscountCode discountCode, string userId);
    }
}
