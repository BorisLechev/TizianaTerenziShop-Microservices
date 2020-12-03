namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class DiscountCodesService : IDiscountCodesService
    {
        private readonly IDeletableEntityRepository<DiscountCode> discountCodesRepository;

        private readonly IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository;

        public DiscountCodesService(
            IDeletableEntityRepository<DiscountCode> discountCodesRepository,
            IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository)
        {
            this.discountCodesRepository = discountCodesRepository;
            this.productsInTheCartRepository = productsInTheCartRepository;
        }

        public async Task<bool> CreateDiscountCodeAsync(DiscountCode discountCode)
        {
            var check = await this.GetDiscountByNameAsync(discountCode.Name);

            if (check == null)
            {
                await this.discountCodesRepository.AddAsync(discountCode);
                var result = await this.discountCodesRepository.SaveChangesAsync();

                return result > 0;
            }

            return false;
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

        public async Task<DiscountCode> GetDiscountByNameAsync(string discountName)
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

        public async Task<bool> ModifyThePricesAfterAppliedDiscountCodeAsync(DiscountCode discountCode, string userId)
        {
            var productsInTheCart = await this.productsInTheCartRepository
                .All()
                .Where(p => p.UserId == userId && p.DiscountCodeId == null)
                //.To<ProductsInTheCartViewModel>()
                .ToListAsync();

            if (productsInTheCart == null)
            {
                return false;
            }

            foreach (var productInTheCart in productsInTheCart)
            {
                productInTheCart.ProductPriceAfterDiscount *= (decimal)(1 - (discountCode.Discount / 100));
                productInTheCart.DiscountCodeId = discountCode.Id;
            }

            await this.productsInTheCartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ModifyThePricesAfterDeletedDiscountCodeAsync(DiscountCode discountCode, string userId)
        {
            var productsInTheCart = await this.productsInTheCartRepository
               .All()
               .Where(p => p.UserId == userId && p.DiscountCodeId != null)
               .Include(p => p.Product)
               .ToListAsync();

            if (productsInTheCart == null)
            {
                return false;
            }

            foreach (var productInTheCart in productsInTheCart)
            {
                productInTheCart.ProductPriceAfterDiscount = productInTheCart.Product.Price;
                productInTheCart.DiscountCodeId = null;
            }

            await this.productsInTheCartRepository.SaveChangesAsync();

            return true;
        }
    }
}
