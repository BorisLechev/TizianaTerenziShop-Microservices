namespace TizianaTerenzi.Services.Data.Discounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.WebClient.ViewModels.DiscountCodes;

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

        public async Task<bool> CreateDiscountCodeAsync(CreateDiscountCodeInputModel inputModel)
        {
            var isExisting = await this.CheckIfThereIsSuchaDiscountAsync(inputModel.Name);

            if (isExisting == false)
            {
                var discountCode = new DiscountCode
                {
                    Name = inputModel.Name,
                    Discount = inputModel.Discount,
                    ExpiresOn = inputModel.ExpiresOn,
                };

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
            var result = await this.discountCodesRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CheckIfThereIsSuchaDiscountAsync(string discountCodeName)
        {
            var isExisting = await this.discountCodesRepository
                .AllAsNoTracking()
                .AnyAsync(dc => dc.Name == discountCodeName && dc.ExpiresOn.Value >= DateTime.UtcNow);

            return isExisting;
        }

        public async Task<IEnumerable<T>> GetAllDiscountCodesAsync<T>()
        {
            var discountCodes = await this.discountCodesRepository
                .AllAsNoTracking()
                .To<T>()
                .ToListAsync();

            return discountCodes;
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

            if (productsInTheCart.Count() == 0)
            {
                return false;
            }

            foreach (var productInTheCart in productsInTheCart)
            {
                productInTheCart.ProductPriceWithDiscountCode *= 1 - ((decimal)discountCode.Discount / 100);
                productInTheCart.DiscountCodeId = discountCode.Id;
            }

            var result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ModifyThePricesAfterDeletedDiscountCodeAsync(string userId)
        {
            var productsInTheCart = await this.productsInTheCartRepository
               .All()
               .Where(p => p.UserId == userId && p.DiscountCodeId != null)
               .Include(p => p.Product) // TODO: Do not use Include
               .ToListAsync();

            if (productsInTheCart.Count() == 0)
            {
                return false;
            }

            foreach (var productInTheCart in productsInTheCart)
            {
                productInTheCart.ProductPriceWithDiscountCode = productInTheCart.Product.PriceWithGeneralDiscount;
                productInTheCart.DiscountCodeId = null;
            }

            var result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
