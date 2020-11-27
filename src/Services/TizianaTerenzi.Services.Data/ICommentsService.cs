namespace TizianaTerenzi.Services.Data
{
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task CreateAsync(int productId, string userId, string content, int? parentId = null);

        Task DeleteRangeAsync(int productId);

        bool IsInProductId(int commentId, int productId);
    }
}
