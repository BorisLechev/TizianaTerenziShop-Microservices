namespace TizianaTerenzi.Products.Services.Data.Votes
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Products.Data.Models;

    public interface ICommentVotesService
    {
        Task<bool> VoteAsync(int commentId, string userId);

        Task<int> GetVotesAsync(int commentId);

        Task<CommentVote> GetVoteAsync(int commentId, string loggedInUserId);

        Task DeleteRangeByProductIdAsync(int productId);

        Task<bool> DeleteRangeByUserIdAsync(string userId);
    }
}
