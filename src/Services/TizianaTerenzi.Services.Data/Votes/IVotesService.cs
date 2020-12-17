namespace TizianaTerenzi.Services.Data.Votes
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface IVotesService
    {
        Task<bool> VoteAsync(int commentId, string userId);

        Task<int> GetVotesAsync(int commentId);

        Task<Vote> GetVoteAsync(int commentId, string loggedInUserId);
    }
}
