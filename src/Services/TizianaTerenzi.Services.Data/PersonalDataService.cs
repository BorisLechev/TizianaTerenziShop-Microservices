namespace TizianaTerenzi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class PersonalDataService : IPersonalDataService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IDeletableEntityRepository<Order> ordersRepository;

        private readonly IDeletableEntityRepository<OrderProduct> orderProductsRepository;

        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        private readonly IDeletableEntityRepository<Vote> votesRepository;

        public PersonalDataService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<OrderProduct> orderProductsRepository,
            IDeletableEntityRepository<Comment> commentsRepository,
            IDeletableEntityRepository<Vote> votesRepository)
        {
            this.usersRepository = usersRepository;

            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
            this.commentsRepository = commentsRepository;
            this.votesRepository = votesRepository;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (userId == null)
            {
                return false;
            }

            var user = await this.usersRepository
                .All()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            try
            {
                var orders = await this.ordersRepository
                    .All()
                    .Where(o => o.UserId == userId)
                    .ToArrayAsync();

                this.ordersRepository.DeleteRange(orders);

                var orderProducts = await this.orderProductsRepository
                    .All()
                    .Where(op => op.UserId == userId)
                    .ToArrayAsync();

                this.orderProductsRepository.DeleteRange(orderProducts);

                var comments = await this.commentsRepository
                    .All()
                    .Where(c => c.UserId == userId)
                    .ToArrayAsync();

                this.commentsRepository.DeleteRange(comments);

                var votes = await this.votesRepository
                    .All()
                    .Where(v => v.UserId == userId)
                    .ToArrayAsync();

                this.votesRepository.DeleteRange(votes);

                this.usersRepository.Delete(user);

                await this.usersRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetPersonalDataForUserJsonAsync(string userId)
        {
            if (userId == null)
            {
                return null;
            }

            var user = await this.usersRepository
                .All()
                .Include(u => u.Orders).ThenInclude(o => o.Products).ThenInclude(o => o.Product)
                .Include(u => u.Comments).ThenInclude(c => c.Votes)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            var personalData = new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                Orders = user.Orders.Select(o => new
                {
                    o.Id,
                    OrderProducts = o.Products.Select(op => new
                    {
                        op.Product.Name,
                        op.Quantity,
                        op.Product.Price,
                        op.CreatedOn,
                        //ProductTypeName = op.Product.ProductType.Name,
                        //FragranceGroupName = op.Product.FragranceGroup.Name,
                        op.Product.YearOfManufacture,
                        op.Product.Description,
                    })
                    .ToArray(),
                })
                .ToArray(),
                Comments = user.Comments.Select(c => new
                {
                    c.Id,
                    c.CreatedOn,
                    c.Content,
                    Votes = c.Votes.Select(v => new
                    {
                        v.Id,
                        v.CommentId,
                        v.CreatedOn,
                        v.Type,
                    })
                    .ToArray(),
                })
                .ToArray(),
            };

            var json = JsonConvert.SerializeObject(personalData, Formatting.Indented);

            return json;
        }
    }
}
