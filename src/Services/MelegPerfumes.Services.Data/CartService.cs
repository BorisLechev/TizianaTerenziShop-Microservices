namespace MelegPerfumes.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;
    using MelegPerfumes.Services.Models;
    using MelegPerfumes.Web.ViewModels.Orders;
    using Microsoft.EntityFrameworkCore;

    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository;

        public CartService(IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository)
        {
            this.productsInTheCartRepository = productsInTheCartRepository;
        }

        public async Task<bool> AddProductInTheCart(ProductInTheCartServiceModel productInTheCartServiceModel)
        {
            if (productInTheCartServiceModel.Quantity < 1)
            {
                throw new ArgumentException(nameof(productInTheCartServiceModel));
            }

            ProductInTheCart productInTheCart = new ProductInTheCart
            {
                Id = productInTheCartServiceModel.Id,
                IssuerId = productInTheCartServiceModel.IssuerId,
                ProductId = productInTheCartServiceModel.ProductId,
                Quantity = productInTheCartServiceModel.Quantity,
            };

            // TODO: Increase quantity of product that already exist in the cart
            //var allProducts = await this.GetAllProductsInTheCartByUserId(productInTheCart.IssuerId);

            //if (allProducts.Any(p => p.ProductId == productInTheCartServiceModel.ProductId))
            //{
            //    productInTheCart.Quantity++;

            //    this.productsInTheCartRepository.Update(productInTheCart);

            //    int resultInner = await this.productsInTheCartRepository.SaveChangesAsync();

            //    return resultInner > 0;
            //}

            await this.productsInTheCartRepository.AddAsync(productInTheCart);

            int result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteProductInTheCart(string orderId)
        {
            var product = await this.productsInTheCartRepository
                .All()
                .SingleOrDefaultAsync(p => p.Id == orderId);

            if (product == null)
            {
                return false;
            }

            this.productsInTheCartRepository.HardDelete(product);
            await this.productsInTheCartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<OrdersCartViewModel>> GetAllProductsInTheCartByUserId(string userId)
        {
            var productsInTheCart = await this.productsInTheCartRepository
                .All()
                .Where(p => p.IssuerId == userId)
                .To<OrdersCartViewModel>()
                .ToListAsync();

            return productsInTheCart;
        }

        public async Task<bool> IncreaseQuantity(string orderId)
        {
            var productInTheCart = await this.productsInTheCartRepository
                .All()
                .SingleOrDefaultAsync(p => p.Id == orderId);

            productInTheCart.Quantity++;

            this.productsInTheCartRepository.Update(productInTheCart);
            int result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ReduceQuantity(string orderId)
        {
            var productInTheCart = await this.productsInTheCartRepository
               .All()
               .SingleOrDefaultAsync(p => p.Id == orderId);

            if (productInTheCart.Quantity <= 1)
            {
                return false;
            }

            productInTheCart.Quantity--;

            this.productsInTheCartRepository.Update(productInTheCart);
            int result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
