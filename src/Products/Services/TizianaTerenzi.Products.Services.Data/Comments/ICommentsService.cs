namespace TizianaTerenzi.Products.Services.Data.Comments
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Products.Web.Models.Comments;

    public interface ICommentsService
    {
        Task<bool> CreateAsync(CreateCommentInputModel inputModel, string userId);

        Task<bool> DeleteRangeByProductIdAsync(int productId);

        Task<bool> DeleteRangeByUserIdAsync(string userId);

        Task<bool> IsInProductIdAsync(int commentId, int productId);

        Task<IEnumerable<UsersCommentsPersonalDataResponseModel>> GetAllUsersCommentsAndVotesPersonalData(string userId);
    }
}
