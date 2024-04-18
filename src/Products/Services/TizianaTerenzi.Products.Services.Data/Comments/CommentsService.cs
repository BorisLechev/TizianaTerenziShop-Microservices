namespace TizianaTerenzi.Products.Services.Data.Comments
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;
    using TizianaTerenzi.Products.Data.Models;
    using TizianaTerenzi.Products.Web.Models.Comments;

    [TransientRegistration]
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

        public async Task<bool> DeleteRangeByProductIdAsync(int productId)
        {
            var comments = await this.commentsRepository
                                .All()
                                .Where(c => c.ProductId == productId)
                                .ExecuteUpdateAsync(setters => setters
                                    .SetProperty(c => c.IsDeleted, true)
                                    .SetProperty(c => c.DeletedOn, DateTime.UtcNow));

            return comments >= 0;
        }

        public async Task<bool> DeleteRangeByUserIdAsync(string userId)
        {
            var affectedRows = await this.commentsRepository
                                    .All()
                                    .Where(c => c.UserId == userId)
                                    .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(c => c.IsDeleted, true)
                                        .SetProperty(c => c.DeletedOn, DateTime.UtcNow));

            return affectedRows >= 0;
        }

        public async Task<bool> IsInProductIdAsync(int commentId, int productId)
        {
            var commentProductId = await this.commentsRepository
                .AllAsNoTracking()
                .Where(c => c.Id == commentId)
                .Select(c => c.ProductId)
                .SingleOrDefaultAsync();

            return commentProductId == productId;
        }

        public async Task<IEnumerable<UsersCommentsPersonalDataResponseModel>> GetAllUsersCommentsAndVotesPersonalData(string userId)
        {
            var usersCommentsPersonalData = await this.commentsRepository
                                                .AllAsNoTracking()
                                                .Where(c => c.UserId == userId)
                                                .Select(c => new UsersCommentsPersonalDataResponseModel
                                                {
                                                    Content = c.Content,
                                                    CreatedOn = c.CreatedOn,
                                                    Votes = c.Votes.Select(cv => new UsersCommentVotesPersonalDataResponseModel
                                                    {
                                                        Type = cv.Type,
                                                        CreatedOn = cv.CreatedOn,
                                                    })
                                                    .ToArray(),
                                                })
                                                .ToListAsync();

            return usersCommentsPersonalData;
        }
    }
}
