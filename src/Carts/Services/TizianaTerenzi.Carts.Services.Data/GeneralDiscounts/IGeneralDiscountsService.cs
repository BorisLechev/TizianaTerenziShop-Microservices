namespace TizianaTerenzi.Carts.Services.Data.GeneralDiscounts
{
    public interface IGeneralDiscountsService
    {
        Task<bool> ModifyThePricesAfterAppliedGeneralDiscountAsync(int discount);

        Task<bool> ModifyThePricesAfterDisabledGeneralDiscountAsync(int discount);
    }
}
