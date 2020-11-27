namespace TizianaTerenzi.Services.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task CreateAsync(int productId, string userId, string content, int? parentId = null)
        {
            var comment = new Comment
            {
                Content = content,
                UserId = userId,
                ProductId = productId,
                ParentId = parentId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(int productId)
        {
            var comments = await this.commentsRepository
                .All()
                .Where(c => c.ProductId == productId)
                .ToListAsync();

            this.commentsRepository.DeleteRange(comments);
            await this.commentsRepository.SaveChangesAsync();
        }

        public bool IsInProductId(int commentId, int productId)
        {
            var commentProductId = this.commentsRepository
                .All()
                .Where(c => c.Id == commentId)
                .Select(c => c.ProductId)
                .SingleOrDefault();

            return commentProductId == productId;
        }
    }
}
