namespace TizianaTerenzi.Services.Data.Discounts
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class GeneralDiscountsService : IGeneralDiscountsService
    {
        private readonly IRepository<GeneralDiscount> generalDiscountsRepository;

        public GeneralDiscountsService(
            IRepository<GeneralDiscount> generalDiscountsRepository)
        {
            this.generalDiscountsRepository = generalDiscountsRepository;
        }

        public async Task<bool> ApplyDiscountToAllProductsAsync(int percent)
        {
            var discount = await this.GetGeneralDiscountAsync();

            discount.Percent = percent;
            discount.IsActive = GeneralDiscountCondition.Active;

            var result = await this.generalDiscountsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DisableDiscountToAllProductsAsync()
        {
            var discount = await this.GetGeneralDiscountAsync();

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
