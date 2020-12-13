namespace TizianaTerenzi.Services.Data
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        Task<bool> CreateAsync(CreateCommentInputModel inputModel, string userId);

        Task DeleteRangeAsync(int productId);

        Task<bool> IsInProductIdAsync(int commentId, int productId);
    }
}
