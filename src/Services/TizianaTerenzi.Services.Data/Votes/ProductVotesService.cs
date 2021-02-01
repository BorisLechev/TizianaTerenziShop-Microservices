namespace TizianaTerenzi.Services.Data.Votes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class ProductVotesService : IProductVotesService
    {
        private readonly IDeletableEntityRepository<ProductVote> productVotesRepository;

        public ProductVotesService(
            IDeletableEntityRepository<ProductVote> productVotesRepository)
        {
            this.productVotesRepository = productVotesRepository;
        }

        public async Task<IEnumerable<byte>> GetAllValuesByProductIdAsync(int productId)
        {
            var voteValues = await this.productVotesRepository
                .AllAsNoTracking()
                .Where(pv => pv.ProductId == productId)
                .Select(pv => pv.Value)
                .ToListAsync();

            return voteValues;
        }

        public async Task<double> GetAverageVotesAsync(int productId)
        {
            return await this.productVotesRepository
                .AllAsNoTracking()
                .Where(pv => pv.ProductId == productId)
                .AverageAsync(pv => pv.Value);
        }

        public async Task<int> GetNumberOfVotersAsync(int productId)
        {
            var numberOfVoters = await this.productVotesRepository
                .AllAsNoTracking()
                .Where(pv => pv.ProductId == productId)
                .CountAsync();

            return numberOfVoters;
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
