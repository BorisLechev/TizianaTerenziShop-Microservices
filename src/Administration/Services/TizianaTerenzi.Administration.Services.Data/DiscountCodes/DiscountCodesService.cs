namespace TizianaTerenzi.Administration.Services.Data.DiscountCodes
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Web.Models.DiscountCodes;
    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Services.Mapping;

    public class DiscountCodesService : IDiscountCodesService
    {
        private readonly IDeletableEntityRepository<DiscountCodeStatistics> discountCodesRepository;
        private readonly IBus publisher;

        public DiscountCodesService(
            IDeletableEntityRepository<DiscountCodeStatistics> discountCodesRepository,
            IBus publisher)
        {
            this.discountCodesRepository = discountCodesRepository;
            this.publisher = publisher;
        }

        public async Task<IEnumerable<T>> GetAllDiscountCodesAsync<T>()
        {
            var discountCodes = await this.discountCodesRepository
                                    .All()
                                    .To<T>()
                                    .ToListAsync();

            return discountCodes;
        }

        public async Task<bool> CreateDiscountCodeAsync(CreateDiscountCodeInputModel inputModel)
        {
            var isExisting = await this.CheckIfThereIsSuchaDiscountAsync(inputModel.Name);

            if (isExisting == false)
            {
                var messageData = new DiscountCodeCreatedMessage
                {
                    Name = inputModel.Name,
                    Discount = inputModel.Discount,
                    ExpiresOn = inputModel.ExpiresOn,
                };

                var message = new EventMessageLog(messageData);

                var discountCode = new DiscountCodeStatistics
                {
                    Name = inputModel.Name,
                    Discount = inputModel.Discount,
                    ExpiresOn = inputModel.ExpiresOn,
                };

                await this.discountCodesRepository.AddAsync(discountCode, message);
                var result = await this.discountCodesRepository.SaveChangesAsync();

                await this.publisher.Publish(message);

                await this.discountCodesRepository.MarkEventMessageLogAsPublished(message.Id);

                return result > 0;
            }

            return false;
        }

        public async Task<bool> CheckIfThereIsSuchaDiscountAsync(string discountCodeName)
        {
            var isExisting = await this.discountCodesRepository
                                    .All()
                                    .AnyAsync(dc => dc.Name == discountCodeName && dc.ExpiresOn.Value >= DateTime.UtcNow);

            return isExisting;
        }

        public async Task<bool> DeleteDiscountCodeAsync(int discountId)
        {
            var discountCode = await this.discountCodesRepository
                                     .All()
                                     .SingleOrDefaultAsync(dc => dc.Id == discountId);

            if (discountCode == null)
            {
                return false;
            }

            this.discountCodesRepository.Delete(discountCode);

            var messageData = new DiscountCodeDeletedMessage
            {
                DiscountCodeId = discountId,
            };

            var message = new EventMessageLog(messageData);

            await this.discountCodesRepository.CreateEventMessageLog(message);

            var result = await this.discountCodesRepository.SaveChangesAsync();

            await this.publisher.Publish(messageData);

            await this.discountCodesRepository.MarkEventMessageLogAsPublished(message.Id);

            return result > 0;
        }
    }
}
