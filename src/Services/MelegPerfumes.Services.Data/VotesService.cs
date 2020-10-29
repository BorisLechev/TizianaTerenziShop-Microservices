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

        public int GetVotes(int commentId)
        {
            var votes = this.votesRepository
                .All()
                .Where(c => c.CommentId == commentId)
                .Sum(v => (int)v.Type); // Type e enum DownVote -1, UpVote 1

            return votes;
        }

        public async Task VoteAsync(int commentId, string userId, bool isUpVote)
        {
            var vote = await this.votesRepository
                .All()
                .SingleOrDefaultAsync(v => v.CommentId == commentId && v.UserId == userId);

            if (vote != null)
            {
                vote.Type = isUpVote ? VoteType.UpVote : VoteType.DownVote;
            }
            else
            {
                vote = new Vote
                {
                    CommentId = commentId,
                    UserId = userId,
                    //Type = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                    Type = VoteType.UpVote,

                };

                await this.votesRepository.AddAsync(vote);
            }

            await this.votesRepository.SaveChangesAsync();
        }
    }
}
