namespace TizianaTerenzi.Services.Data.Votes
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class VotesService : IVotesService
    {
        private readonly IDeletableEntityRepository<Vote> votesRepository;

        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public VotesService(
            IDeletableEntityRepository<Vote> votesRepository,
            IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.votesRepository = votesRepository;
            this.commentsRepository = commentsRepository;
        }

        public async Task DeleteRangeAsync(int productId)
        {
            var commentIds = await this.commentsRepository
                .AllAsNoTracking()
                .Where(c => c.ProductId == productId)
                .Select(c => c.Id)
                .ToListAsync();

            var votes = await this.votesRepository
                .All()
                .Where(v => commentIds.Contains(v.CommentId))
                .ToListAsync();

            if (votes.Any())
            {
                this.votesRepository.DeleteRange(votes);
                await this.votesRepository.SaveChangesAsync();
            }
        }

        public async Task<Vote> GetVoteAsync(int commentId, string loggedInUserId)
        {
            var vote = await this.votesRepository
                .All()
                .SingleOrDefaultAsync(v => v.CommentId == commentId && v.UserId == loggedInUserId);

            return vote;
        }

        public async Task<int> GetVotesAsync(int commentId)
        {
            var votesSum = await this.votesRepository
                .All()
                .Where(c => c.CommentId == commentId)
                .SumAsync(v => (int)v.Type); // Type e enum DownVote -1, UpVote 1

            return votesSum;
        }

        public async Task<bool> VoteAsync(int commentId, string userId)
        {
            var vote = await this.votesRepository
                .All()
                .SingleOrDefaultAsync(v => v.CommentId == commentId && v.UserId == userId);

            if (vote != null && vote.Type == VoteType.UpVote)
            {
                vote.Type = VoteType.Neutral;
            }
            else if (vote != null && vote.Type == VoteType.Neutral)
            {
                vote.Type = VoteType.UpVote;
            }
            else
            {
                vote = new Vote
                {
                    CommentId = commentId,
                    UserId = userId,
                    Type = VoteType.UpVote,
                };

                await this.votesRepository.AddAsync(vote);
            }

            var result = await this.votesRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
