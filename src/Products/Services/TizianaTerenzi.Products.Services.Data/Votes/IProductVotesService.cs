namespace TizianaTerenzi.Products.Services.Data.Votes
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Products.Web.Models.Votes;

    public interface IProductVotesService
    {
        Task<bool> VoteAsync(int productId, string userId, byte value);

        Task<GroupProductVoteValuesViewModel<int, int, int, int, int, int, int, double>> GetNumberOfVotesForEachValueAsync(int productId);

        Task DeleteProductVotesAsync(int productId);
    }
}
