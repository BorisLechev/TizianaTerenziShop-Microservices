namespace TizianaTerenzi.Services.Data.Votes
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.WebClient.ViewModels.Votes;

    public class ProductVotesService : IProductVotesService
    {
        private readonly IDeletableEntityRepository<ProductVote> productVotesRepository;

        public ProductVotesService(
            IDeletableEntityRepository<ProductVote> productVotesRepository)
        {
            this.productVotesRepository = productVotesRepository;
        }

        public async Task DeleteProductVotesAsync(int productId)
        {
            var votes = await this.productVotesRepository
                    .All()
                    .Where(v => v.ProductId == productId)
                    .ToArrayAsync();

            if (votes.Any())
            {
                this.productVotesRepository.DeleteRange(votes);
                await this.productVotesRepository.SaveChangesAsync();
            }
        }

        public async Task<GroupProductVoteValuesViewModel<int, int, int, int, int, int, int, double>> GetNumberOfVotesForEachValueAsync(int productId)
        {
            var votes = await this.productVotesRepository
                        .AllAsNoTracking()
                        .Where(pv => pv.ProductId == productId)
                        .GroupBy(pv => pv.ProductId)
                        .Select(pv => new GroupProductVoteValuesViewModel<int, int, int, int, int, int, int, double>
                        {
                            Group = pv.Key,
                            GroupVotesWithValue5 = pv.Where(v => v.Value == 5).Count(),
                            GroupVotesWithValue4 = pv.Where(v => v.Value == 4).Count(),
                            GroupVotesWithValue3 = pv.Where(v => v.Value == 3).Count(),
                            GroupVotesWithValue2 = pv.Where(v => v.Value == 2).Count(),
                            GroupVotesWithValue1 = pv.Where(v => v.Value == 1).Count(),
                            CountOfVotes = pv.Count(),
                            AverageVotes = pv.Average(v => v.Value),
                        })
                        .SingleOrDefaultAsync();

            return votes;
        }

        public async Task<bool> VoteAsync(int productId, string userId, byte value)
        {
            var vote = await this.productVotesRepository
                .All()
                .SingleOrDefaultAsync(pv => pv.ProductId == productId && pv.UserId == userId);

            if (vote == null)
            {
                vote = new ProductVote
                {
                    ProductId = productId,
                    UserId = userId,
                };

                await this.productVotesRepository.AddAsync(vote);
            }

            vote.Value = value;
            var result = await this.productVotesRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
