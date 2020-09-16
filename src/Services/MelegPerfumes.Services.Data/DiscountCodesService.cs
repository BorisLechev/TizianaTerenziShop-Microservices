namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class DiscountCodesService : IDiscountCodesService
    {
        private readonly IDeletableEntityRepository<DiscountCode> discountCodesRepository;

        public DiscountCodesService(
            IDeletableEntityRepository<DiscountCode> discountCodesRepository)
        {
            this.discountCodesRepository = discountCodesRepository;
        }

        public async Task<bool> CreateDiscountCodeAsync(DiscountCode discountCode)
        {
            // TODO: Check if exists
            await this.discountCodesRepository.AddAsync(discountCode);
            await this.discountCodesRepository.SaveChangesAsync();

            return true;
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
            await this.discountCodesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<DiscountCode> FindDiscountByNameAsync(string discountName)
        {
            var discountCode = await this.discountCodesRepository
                .All()
                .SingleOrDefaultAsync(dc => dc.Name == discountName);

            return discountCode;
        }

        public async Task<IEnumerable<DiscountCode>> GetAllDiscountCodesAsync()
        {
            var discountCodes = await this.discountCodesRepository
                .All()
                .ToListAsync();

            return discountCodes;
        }
    }
}
