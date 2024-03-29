namespace TizianaTerenzi.Carts.Services.Data.GeneralDiscounts
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;

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
                                    .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(p => p.Price, p => p.Price - (p.Price * discount / 100))
                                        .SetProperty(p => p.ModifiedOn, DateTime.UtcNow));

            return affectedRows > 0;
        }

        public async Task<bool> ModifyThePricesAfterDisabledGeneralDiscountAsync(int discount)
        {
            var affectedRows = await this.productsInTheCartRepository
                                    .All()
                                    .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(p => p.Price, p => p.Price / (1 - ((decimal)discount / 100)))
                                        .SetProperty(p => p.ModifiedOn, DateTime.UtcNow));

            return affectedRows > 0;
        }
    }
}
