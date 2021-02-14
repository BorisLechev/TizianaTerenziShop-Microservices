namespace TizianaTerenzi.Services.Data.Comments
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        Task<bool> CreateAsync(CreateCommentInputModel inputModel, string userId);

        Task DeleteRangeByProductIdAsync(int productId);

        Task<bool> DeleteRangeByUserIdAsync(string userId);

        Task<bool> IsInProductIdAsync(int commentId, int productId);
    }
}
