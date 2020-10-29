using MelegPerfumes.Data.Common.Repositories;
using MelegPerfumes.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelegPerfumes.Services.Data
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task Create(int productId, string userId, string content, int? parentId = null)
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
