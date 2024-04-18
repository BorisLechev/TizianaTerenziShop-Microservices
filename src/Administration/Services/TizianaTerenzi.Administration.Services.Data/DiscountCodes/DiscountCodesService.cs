namespace TizianaTerenzi.Administration.Services.Data.DiscountCodes
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Web.Models.DiscountCodes;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;

    [TransientRegistration]
    public class DiscountCodesService : IDiscountCodesService
    {
        private readonly IDeletableEntityRepository<DiscountCodeStatistics> discountCodesRepository;

        public DiscountCodesService(
            IDeletableEntityRepository<DiscountCodeStatistics> discountCodesRepository)
        {
            this.discountCodesRepository = discountCodesRepository;
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
                var message = new DiscountCodeCreatedMessage
                {
                    Name = inputModel.Name,
                    Discount = inputModel.Discount,
                    ExpiresOn = inputModel.ExpiresOn,
                };

                var discountCode = new DiscountCodeStatistics
                {
                    Name = inputModel.Name,
                    Discount = inputModel.Discount,
                    ExpiresOn = inputModel.ExpiresOn,
                };

                await this.discountCodesRepository.AddAsync(discountCode);
                var result = await this.discountCodesRepository.SaveAndPublishEventMessageAsync(message);

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

            var message = new DiscountCodeDeletedMessage
            {
                DiscountCodeId = discountId,
            };

            var result = await this.discountCodesRepository.SaveAndPublishEventMessageAsync(message);

            return result > 0;
        }
    }
}
