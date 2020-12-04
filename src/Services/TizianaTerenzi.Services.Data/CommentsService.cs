namespace TizianaTerenzi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Comments;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task<bool> CreateAsync(CreateCommentInputModel inputModel, string userId)
        {
            var comment = new Comment
            {
                Content = inputModel.Content,
                UserId = userId,
                ProductId = inputModel.ProductId,
                ParentId = inputModel.ParentId,
            };

            await this.commentsRepository.AddAsync(comment);
            var result = await this.commentsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task DeleteRangeAsync(int productId)
        {
            var comments = await this.commentsRepository
                .All()
                .Where(c => c.ProductId == productId)
                .ToListAsync();

            if (comments.Any())
            {
                this.commentsRepository.DeleteRange(comments);
                await this.commentsRepository.SaveChangesAsync();
            }
        }

        public async Task<bool> IsInProductIdAsync(int commentId, int productId)
        {
            var commentProductId = await this.commentsRepository
                .All()
                .Where(c => c.Id == commentId)
                .Select(c => c.ProductId)
                .SingleOrDefaultAsync();

            return commentProductId == productId;
        }
    }
}
