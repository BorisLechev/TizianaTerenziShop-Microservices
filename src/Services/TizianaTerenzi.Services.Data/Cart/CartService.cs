namespace TizianaTerenzi.Services.Data.Cart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Orders;

    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository;

        private readonly IDeletableEntityRepository<Order> ordersRepository;

        private readonly IOrderStatusesService orderStatusesService;

        public CartService(
            IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository,
            IDeletableEntityRepository<Order> ordersRepository,
            IOrderStatusesService orderStatusesService)
        {
            this.productsInTheCartRepository = productsInTheCartRepository;
            this.ordersRepository = ordersRepository;
            this.orderStatusesService = orderStatusesService;
        }

        public async Task<bool> AddProductInTheCart(Product product, string userId)
        {
            var productInTheCart = new ProductInTheCart
            {
                Id = Guid.NewGuid().ToString(), // TODO: make it int
                UserId = userId,
                ProductId = product.Id,
                Quantity = 1,
                ProductPriceAfterDiscount = product.Price,
            };

            await this.productsInTheCartRepository.AddAsync(productInTheCart);

            int result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CheckIfProductByUserIdExistInTheCartAsync(string userId, int productId)
        {
            var result = await this.productsInTheCartRepository
                .AllAsNoTracking()
                .Where(p => p.UserId == userId)
                .AnyAsync(p => p.ProductId == productId);

            return result;
        }

        public async Task<Order> CheckOutAsync(string userId, IEnumerable<ProductsInTheCartViewModel> productsInTheCart)
        {
            var orderProducts = productsInTheCart
                .Select(op => new OrderProduct
                {
                    ProductId = op.ProductId,
                    Price = op.ProductPriceAfterDiscount,
                    Quantity = op.Quantity,
                    CreatedOn = DateTime.UtcNow,
                    UserId = userId,
                    DiscountCodeId = op.DiscountCodeId,
                })
                .ToList();

            var discountCodeId = productsInTheCart.FirstOrDefault().DiscountCodeId;

            var pendingStatus = await this.orderStatusesService
                .FindByNameAsync("Pending");

            var order = new Order
            {
                UserId = userId,
                StatusId = pendingStatus.Id,
                Status = pendingStatus,
                Products = orderProducts,
                DiscountCodeId = discountCodeId,
            };

            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();

            var createdOrder = await this.ordersRepository
                .All()
                .Include(o => o.Products) // TODO: do not use Include
                .ThenInclude(o => o.Product)
                .SingleOrDefaultAsync(o => o.Id == order.Id);

            return createdOrder;
        }

        public async Task<bool> DeleteAllProductsInTheCartByUserId(string userId)
        {
            var products = await this.productsInTheCartRepository
                .All()
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (products == null)
            {
                return false;
            }

            this.productsInTheCartRepository.HardDeleteRange(products);
            await this.productsInTheCartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductInTheCart(string productId)
        {
            var product = await this.productsInTheCartRepository
                .All()
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return false;
            }

            this.productsInTheCartRepository.HardDelete(product);
            await this.productsInTheCartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCartByUserId(string userId)
        {
            var productsInTheCart = await this.productsInTheCartRepository
                .All()
                .Where(p => p.UserId == userId)
                .To<ProductsInTheCartViewModel>()
                .ToListAsync();

            return productsInTheCart;
        }

        public async Task<string> GetProductInTheCartIdByProductIdAsync(int productId)
        {
            var productInTheCartId = await this.productsInTheCartRepository
                .AllAsNoTracking()
                .Where(p => p.ProductId == productId)
                .Select(p => p.Id)
                .SingleOrDefaultAsync();

            return productInTheCartId;
        }

        public async Task<bool> IncreaseQuantity(string productId)
        {
            var productInTheCart = await this.productsInTheCartRepository
                .All()
                .SingleOrDefaultAsync(p => p.Id == productId);

            productInTheCart.Quantity++;

            int result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ReduceQuantity(string productId)
        {
            var productInTheCart = await this.productsInTheCartRepository
               .All()
               .SingleOrDefaultAsync(p => p.Id == productId);

            if (productInTheCart.Quantity <= 1)
            {
                return false;
            }

            productInTheCart.Quantity--;

            int result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
