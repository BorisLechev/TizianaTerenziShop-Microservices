namespace TizianaTerenzi.Administration.Services.Data.GeneralDiscounts
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;

    [TransientRegistration]
    public class GeneralDiscountsService : IGeneralDiscountsService
    {
        private readonly IRepository<GeneralDiscount> generalDiscountsRepository;

        public GeneralDiscountsService(
            IRepository<GeneralDiscount> generalDiscountsRepository)
        {
            this.generalDiscountsRepository = generalDiscountsRepository;
        }

        public async Task<bool> ApplyDiscountToAllProductsAsync(byte percent)
        {
            var affectedRows = await this.generalDiscountsRepository
                                    .All()
                                    .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(gd => gd.Percent, percent)
                                        .SetProperty(gd => gd.IsActive, GeneralDiscountCondition.Active));

            var messageProductPricesUpdated = new ThePricesOfAllProductsAfterTheGeneralDiscountIsAppliedUpdatedMessage
            {
                Discount = percent,
            };

            var messageProductsPricesInTheCartsUpdated = new ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsAppliedUpdatedMessage
            {
                Discount = percent,
            };

            await this.generalDiscountsRepository.SaveAndPublishEventMessageAsync(messageProductPricesUpdated, messageProductsPricesInTheCartsUpdated);

            return affectedRows > 0;
        }

        public async Task<bool> DisableDiscountToAllProductsAsync()
        {
            var discount = await this.GetGeneralDiscountAsync();

            discount.IsActive = GeneralDiscountCondition.Inactive;

            var messageProductPricesUpdated = new ThePricesOfAllProductsAfterTheGeneralDiscountIsDisabledUpdatedMessage();

            var messageProductsPricesInTheCartsUpdated = new ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsDisabledUpdatedMessage
            {
                Discount = discount.Percent,
            };

            var result = await this.generalDiscountsRepository.SaveAndPublishEventMessageAsync(messageProductPricesUpdated, messageProductsPricesInTheCartsUpdated);

            return result > 0;
        }

        public async Task<GeneralDiscount> GetGeneralDiscountAsync()
        {
            var discount = await this.generalDiscountsRepository
                .All()
                .SingleOrDefaultAsync();

            return discount;
        }

        public async Task<T> GetGeneralDiscountAsync<T>()
        {
            var discount = await this.generalDiscountsRepository
                .All()
                .To<T>()
                .SingleOrDefaultAsync();

            return discount;
        }
    }
}
