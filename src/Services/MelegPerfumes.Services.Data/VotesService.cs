namespace MelegPerfumes.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
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
            var votes = await this.votesRepository
                .All()
                .Where(c => c.CommentId == commentId)
                .SumAsync(v => (int)v.Type); // Type e enum DownVote -1, UpVote 1

            return votes;
        }

        public async Task VoteAsync(int commentId, string userId)
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

            await this.votesRepository.SaveChangesAsync();
        }
    }
}
