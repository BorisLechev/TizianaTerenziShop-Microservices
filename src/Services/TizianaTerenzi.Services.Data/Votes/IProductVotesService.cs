namespace TizianaTerenzi.Services.Data.Votes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface IProductVotesService
    {
        Task<bool> VoteAsync(int productId, string userId, byte value);

        Task<double> GetAverageVotesAsync(int productId);

        Task<int> GetNumberOfVotersAsync(int productId);

        Task<IEnumerable<byte>> GetAllValuesByProductIdAsync(int productId);
    }
}
