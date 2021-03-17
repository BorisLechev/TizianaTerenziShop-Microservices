namespace TizianaTerenzi.Services.Data.Cart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
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

        private readonly UserManager<ApplicationUser> userManager;

        public CartService(
            IDeletableEntityRepository<ProductInTheCart> productsInTheCartRepository,
            IDeletableEntityRepository<Order> ordersRepository,
            IOrderStatusesService orderStatusesService,
            UserManager<ApplicationUser> userManager)
        {
            this.productsInTheCartRepository = productsInTheCartRepository;
            this.ordersRepository = ordersRepository;
            this.orderStatusesService = orderStatusesService;
            this.userManager = userManager;
        }

        public async Task<bool> AddProductInTheCart(Product product, string userId)
        {
            var productInTheCart = new ProductInTheCart
            {
                UserId = userId,
                ProductId = product.Id,
                Quantity = 1,
                ProductPriceAfterDiscount = product.PriceWithDiscount,
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

        public async Task<bool> CheckoutAsync(string userId, IEnumerable<ProductsInTheCartViewModel> productsInTheCart)
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

            var pendingStatusId = await this.orderStatusesService.FindByNameAsync("Pending");

            var order = new Order
            {
                UserId = userId,
                StatusId = pendingStatusId,
                Products = orderProducts,
                DiscountCodeId = discountCodeId,
            };

            await this.ordersRepository.AddAsync(order);
            var result = await this.ordersRepository.SaveChangesAsync();

            return result > 0;
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

        public async Task SaveShippingDataAsync(ApplicationUser user, ShippingDataInputModel inputModel)
        {
            user.CountryId = inputModel.CountryId;
            user.Address = inputModel.Address;
            user.Town = inputModel.Town;
            user.PostalCode = inputModel.PostalCode;
            user.PhoneNumber = inputModel.PhoneNumber;

            await this.userManager.UpdateAsync(user);
        }
    }
}
