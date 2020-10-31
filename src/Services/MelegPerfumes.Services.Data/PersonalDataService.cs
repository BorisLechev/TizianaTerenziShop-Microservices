namespace MelegPerfumes.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class PersonalDataService : IPersonalDataService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public PersonalDataService(
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<string> GetPersonalDataForUserJson(string userId)
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
