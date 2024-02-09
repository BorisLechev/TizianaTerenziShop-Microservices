namespace TizianaTerenzi.Administration.Services.Data.GeneralDiscounts
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Services.Mapping;

    public class GeneralDiscountsService : IGeneralDiscountsService
    {
        private readonly IRepository<GeneralDiscount> generalDiscountsRepository;
        private readonly IBus publisher;

        public GeneralDiscountsService(
            IRepository<GeneralDiscount> generalDiscountsRepository,
            IBus publisher)
        {
            this.generalDiscountsRepository = generalDiscountsRepository;
            this.publisher = publisher;
        }

        public async Task<bool> ApplyDiscountToAllProductsAsync(byte percent)
        {
            await this.publisher.PublishBatch(new object[]
            {
                new ThePricesOfAllProductsAfterTheGeneralDiscountIsAppliedUpdatedMessage
                {
                    Discount = percent,
                },
                new ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsAppliedUpdatedMessage
                {
                    Discount = percent,
                },
            });

            var discount = await this.GetGeneralDiscountAsync();

            discount.Percent = percent;
            discount.IsActive = GeneralDiscountCondition.Active;

            var result = await this.generalDiscountsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DisableDiscountToAllProductsAsync()
        {
            var discount = await this.GetGeneralDiscountAsync();

            await this.publisher.PublishBatch(new object[]
            {
                new ThePricesOfAllProductsAfterTheGeneralDiscountIsDisabledUpdatedMessage(),
                new ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsDisabledUpdatedMessage
                {
                    Discount = discount.Percent,
                },
            });

            discount.IsActive = GeneralDiscountCondition.Inactive;

            var result = await this.generalDiscountsRepository.SaveChangesAsync();

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
