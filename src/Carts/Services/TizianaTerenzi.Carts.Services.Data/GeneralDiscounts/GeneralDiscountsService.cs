namespace TizianaTerenzi.Carts.Services.Data.GeneralDiscounts
{
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using Z.EntityFramework.Plus;

    public class GeneralDiscountsService : IGeneralDiscountsService
    {
        private readonly IDeletableEntityRepository<Cart> productsInTheCartRepository;

        public GeneralDiscountsService(IDeletableEntityRepository<Cart> productsInTheCartRepository)
        {
            this.productsInTheCartRepository = productsInTheCartRepository;
        }

        public async Task<bool> ModifyThePricesAfterAppliedGeneralDiscountAsync(int discount)
        {
            var affectedRows = await this.productsInTheCartRepository
                                    .All()
                                    .UpdateAsync(p => new Cart
                                    {
                                        ProductPriceWithGeneralDiscount = p.ProductPriceWithGeneralDiscount - (p.ProductPriceWithGeneralDiscount * discount / 100),
                                        PriceWithDiscountCode = p.PriceWithDiscountCode - (p.PriceWithDiscountCode * discount / 100),
                                        ModifiedOn = DateTime.UtcNow,
                                    });

            return affectedRows > 0;
        }

        public async Task<bool> ModifyThePricesAfterDisabledGeneralDiscountAsync(int discount)
        {
            var affectedRows = await this.productsInTheCartRepository
                                    .All()
                                    .UpdateAsync(p => new Cart
                                    {
                                        ProductPriceWithGeneralDiscount = p.ProductPriceWithGeneralDiscount / (1 - ((decimal)discount / 100)),
                                        PriceWithDiscountCode = p.PriceWithDiscountCode / (1 - ((decimal)discount / 100)),
                                        ModifiedOn = DateTime.UtcNow,
                                    });

            return affectedRows > 0;
        }
    }
}
