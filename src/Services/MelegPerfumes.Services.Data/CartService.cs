namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;
    using MelegPerfumes.Web.ViewModels.Orders;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> AddProductInTheCart(ProductInTheCart productInTheCart)
        {
            await this.productsInTheCartRepository.AddAsync(productInTheCart);

            int result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CheckIfProductByUserIdExistInTheCartAsync(string userId, int productId)
        {
            var product = await this.productsInTheCartRepository
                .All()
                .Where(p => p.UserId == userId)
                .SingleOrDefaultAsync(p => p.ProductId == productId);

            return product != null ? true : false;
        }

        public async Task<Order> CheckOutAsync(string userId, ICollection<OrderProduct> orderProducts)
        {
            var pendingStatus = await this.orderStatusesService
                .FindByNameAsync("Pending");

            var order = new Order
            {
                UserId = userId,
                StatusId = pendingStatus.Id,
                Products = orderProducts,
            };

            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();

            var createdOrder = await this.ordersRepository
                .All()
                .Include(o => o.Products)
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

        public async Task<ProductsInTheCartViewModel> GetProductById(int productId)
        {
            var productInTheCart = await this.productsInTheCartRepository
                .All()
                .Where(p => p.ProductId == productId)
                .To<ProductsInTheCartViewModel>()
                .SingleOrDefaultAsync();

            return productInTheCart;
        }

        public async Task<bool> IncreaseQuantity(string productId)
        {
            var productInTheCart = await this.productsInTheCartRepository
                .All()
                .SingleOrDefaultAsync(p => p.Id == productId);

            productInTheCart.Quantity++;

            this.productsInTheCartRepository.Update(productInTheCart);
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

            this.productsInTheCartRepository.Update(productInTheCart);
            int result = await this.productsInTheCartRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
