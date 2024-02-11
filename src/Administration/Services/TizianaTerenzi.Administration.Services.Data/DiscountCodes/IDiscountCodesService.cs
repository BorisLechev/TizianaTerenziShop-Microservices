namespace TizianaTerenzi.Administration.Services.Data.DiscountCodes
{
    using TizianaTerenzi.Administration.Web.Models.DiscountCodes;

    public interface IDiscountCodesService
    {
        Task<IEnumerable<T>> GetAllDiscountCodesAsync<T>();

        Task<bool> CreateDiscountCodeAsync(CreateDiscountCodeInputModel inputModel);

        Task<bool> CheckIfThereIsSuchaDiscountAsync(string discountCodeName);

        Task<bool> DeleteDiscountCodeAsync(int discountId);
    }
}
