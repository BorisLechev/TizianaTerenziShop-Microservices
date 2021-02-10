namespace TizianaTerenzi.Services.Data.Votes
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class CommentVotesService : ICommentVotesService
    {
        private readonly IDeletableEntityRepository<CommentVote> commentVotesRepository;

        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentVotesService(
            IDeletableEntityRepository<CommentVote> commentVotesRepository,
            IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentVotesRepository = commentVotesRepository;
            this.commentsRepository = commentsRepository;
        }

        public async Task DeleteRangeByProductIdAsync(int productId)
        {
            var commentIds = await this.commentsRepository
                .AllAsNoTracking()
                .Where(c => c.ProductId == productId)
                .Select(c => c.Id)
                .ToListAsync();

            var votes = await this.commentVotesRepository
                .All()
                .Where(v => commentIds.Contains(v.CommentId))
                .ToListAsync();

            if (votes.Any())
            {
                this.commentVotesRepository.DeleteRange(votes);
                await this.commentVotesRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeByUserIdAsync(string userId)
        {
            var votes = await this.commentVotesRepository
                    .All()
                    .Where(v => v.UserId == userId)
                    .ToArrayAsync();

            if (votes.Any())
            {
                this.commentVotesRepository.DeleteRange(votes);
                await this.commentVotesRepository.SaveChangesAsync();
            }
        }

        public async Task<CommentVote> GetVoteAsync(int commentId, string loggedInUserId)
        {
            var vote = await this.commentVotesRepository
                .All()
                .SingleOrDefaultAsync(v => v.CommentId == commentId && v.UserId == loggedInUserId);

            return vote;
        }

        public async Task<int> GetVotesAsync(int commentId)
        {
            var votesSum = await this.commentVotesRepository
                .All()
                .Where(c => c.CommentId == commentId)
                .SumAsync(v => (int)v.Type); // Type e enum DownVote -1, UpVote 1

            return votesSum;
        }

        public async Task<bool> VoteAsync(int commentId, string userId)
        {
            var vote = await this.commentVotesRepository
                .All()
                .SingleOrDefaultAsync(v => v.CommentId == commentId && v.UserId == userId);

            if (vote != null && vote.Type == CommentVoteType.UpVote)
            {
                vote.Type = CommentVoteType.Neutral;
            }
            else if (vote != null && vote.Type == CommentVoteType.Neutral)
            {
                vote.Type = CommentVoteType.UpVote;
            }
            else
            {
                vote = new CommentVote
                {
                    CommentId = commentId,
                    UserId = userId,
                    Type = CommentVoteType.UpVote,
                };

                await this.commentVotesRepository.AddAsync(vote);
            }

            var result = await this.commentVotesRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
