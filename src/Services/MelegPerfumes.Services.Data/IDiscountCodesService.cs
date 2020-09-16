namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;

    public interface IDiscountCodesService
    {
        Task<bool> CreateDiscountCodeAsync(DiscountCode discountCode);

        Task<bool> DeleteDiscountCodeAsync(int discountId);

        Task<IEnumerable<DiscountCode>> GetAllDiscountCodesAsync();

        Task<DiscountCode> FindDiscountByNameAsync(string discountName);
    }
}
