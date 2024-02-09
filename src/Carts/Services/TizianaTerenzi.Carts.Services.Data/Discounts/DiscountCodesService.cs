namespace TizianaTerenzi.Carts.Services.Data.Discounts
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;

    public class DiscountCodesService : IDiscountCodesService
    {
        private readonly IDeletableEntityRepository<DiscountCode> discountCodesRepository;

        private readonly IDeletableEntityRepository<Cart> productsInTheCartRepository;

        public DiscountCodesService(
            IDeletableEntityRepository<DiscountCode> discountCodesRepository,
            IDeletableEntityRepository<Cart> productsInTheCartRepository)
        {
            this.discountCodesRepository = discountCodesRepository;
            this.productsInTheCartRepository = productsInTheCartRepository;
        }

        public async Task<bool> CheckIfThereIsSuchaDiscountAsync(string discountCodeName)
        {
            var isExisting = await this.discountCodesRepository
                                    .AllAsNoTracking()
                                    .AnyAsync(dc => dc.Name == discountCodeName && dc.ExpiresOn.Value >= DateTime.UtcNow);

            return isExisting;
        }

        public async Task<DiscountCode> GetDiscountCodeByNameAsync(string discountCodeName)
        {
            var discountCode = await this.discountCodesRepository
                                     .All()
                                     .SingleOrDefaultAsync(dc => dc.Name == discountCodeName);

            return discountCode;
        }

        public async Task<bool> ModifyThePricesAfterAppliedDiscountCodeAsync(string discountCodeName, string userId)
        {
            var discountCode = await this.GetDiscountCodeByNameAsync(discountCodeName);
            var productsInTheCart = await this.productsInTheCartRepository
                                        .All()
                                        .Where(p => p.UserId == userId && p.DiscountCodeId == null)
                                        .ToListAsync();

            if (!productsInTheCart.Any())
            {
                return false;
            }

            foreach (var productInTheCart in productsInTheCart)
            {
                productInTheCart.Price *= 1 - ((decimal)discountCode.Discount / 100);
                productInTheCart.DiscountCodeId = discountCode.Id;
                productInTheCart.ModifiedOn = DateTime.UtcNow;
            }

            var result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ModifyThePricesAfterDeletedDiscountCodeAsync(string discountCodeName,  string userId)
        {
            var productsInTheCart = await this.productsInTheCartRepository
                                        .All()
                                        .Where(p => p.UserId == userId && p.DiscountCodeId != null)
                                        .ToListAsync();

            if (!productsInTheCart.Any())
            {
                return false;
            }

            var discountCode = await this.GetDiscountCodeByNameAsync(discountCodeName);

            foreach (var productInTheCart in productsInTheCart)
            {
                productInTheCart.Price /= 1 - ((decimal)discountCode.Discount / 100);
                productInTheCart.DiscountCodeId = null;
                productInTheCart.ModifiedOn = DateTime.UtcNow;
            }

            var result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
