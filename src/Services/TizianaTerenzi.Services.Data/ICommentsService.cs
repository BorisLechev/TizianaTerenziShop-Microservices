namespace TizianaTerenzi.Services.Data
{
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task Create(int productId, string userId, string content, int? parentId = null);

        bool IsInProductId(int commentId, int productId);
    }
}
