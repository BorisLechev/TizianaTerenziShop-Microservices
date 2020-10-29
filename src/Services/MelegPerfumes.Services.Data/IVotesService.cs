namespace MelegPerfumes.Services.Data
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task VoteAsync(int commentId, string userId, bool isUpVote);

        int GetVotes(int commentId);
    }
}
