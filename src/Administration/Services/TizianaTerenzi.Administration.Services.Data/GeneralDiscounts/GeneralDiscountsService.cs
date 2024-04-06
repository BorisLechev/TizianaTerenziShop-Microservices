namespace TizianaTerenzi.Administration.Services.Data.GeneralDiscounts
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Models;
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
            var affectedRows = await this.generalDiscountsRepository
                                    .All()
                                    .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(gd => gd.Percent, percent)
                                        .SetProperty(gd => gd.IsActive, GeneralDiscountCondition.Active));

            var messageDataProductPricesUpdated = new ThePricesOfAllProductsAfterTheGeneralDiscountIsAppliedUpdatedMessage
            {
                Discount = percent,
            };

            var messageDataProductsPricesInTheCartsUpdated = new ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsAppliedUpdatedMessage
            {
                Discount = percent,
            };

            var messageProductPricesUpdated = new EventMessageLog(messageDataProductPricesUpdated);
            var messageProductsPricesInTheCartsUpdated = new EventMessageLog(messageDataProductsPricesInTheCartsUpdated);

            await this.generalDiscountsRepository.CreateEventMessageLog(messageProductPricesUpdated, messageProductsPricesInTheCartsUpdated);
            await this.generalDiscountsRepository.SaveChangesAsync();

            await this.publisher.PublishBatch(new object[]
            {
                messageDataProductPricesUpdated,
                messageDataProductsPricesInTheCartsUpdated,
            });

            await this.generalDiscountsRepository.MarkEventMessageLogAsPublished(messageProductPricesUpdated.Id, messageProductsPricesInTheCartsUpdated.Id);

            return affectedRows > 0;
        }

        public async Task<bool> DisableDiscountToAllProductsAsync()
        {
            var discount = await this.GetGeneralDiscountAsync();

            discount.IsActive = GeneralDiscountCondition.Inactive;

            var result = await this.generalDiscountsRepository.SaveChangesAsync();

            var messageDataProductPricesUpdated = new ThePricesOfAllProductsAfterTheGeneralDiscountIsDisabledUpdatedMessage();

            var messageDataProductsPricesInTheCartsUpdated = new ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsDisabledUpdatedMessage
            {
                Discount = discount.Percent,
            };

            var messageProductPricesUpdated = new EventMessageLog(messageDataProductPricesUpdated);
            var messageProductsPricesInTheCartsUpdated = new EventMessageLog(messageDataProductsPricesInTheCartsUpdated);

            await this.generalDiscountsRepository.CreateEventMessageLog(messageProductPricesUpdated, messageProductsPricesInTheCartsUpdated);
            await this.generalDiscountsRepository.SaveChangesAsync();

            await this.publisher.PublishBatch(new object[]
            {
                messageDataProductPricesUpdated,
                messageDataProductsPricesInTheCartsUpdated,
            });

            await this.generalDiscountsRepository.MarkEventMessageLogAsPublished(messageProductPricesUpdated.Id, messageProductsPricesInTheCartsUpdated.Id);

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
