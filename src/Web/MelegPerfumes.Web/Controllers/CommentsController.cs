using MelegPerfumes.Data.Models;
using MelegPerfumes.Services.Data;
using MelegPerfumes.Web.ViewModels.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MelegPerfumes.Web.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ICommentsService commentsService;

        public CommentsController(
            UserManager<ApplicationUser> userManager,
            ICommentsService commentsService)
        {
            this.userManager = userManager;
            this.commentsService = commentsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCommentInputModel inputModel)
        {
            var parentId = inputModel.ParentId == 0 ? (int?)null : inputModel.ParentId;

            if (parentId.HasValue)
            {
                if (!this.commentsService.IsInProductId(parentId.Value, inputModel.ProductId))
                {
                    return this.BadRequest(); // TODO: 
                }
            }

            var userId = this.userManager.GetUserId(this.User);

            await this.commentsService.Create(inputModel.ProductId, userId, inputModel.Content, parentId);

            return this.RedirectToAction("Details", "Products", new { id = inputModel.ProductId });
        }
    }
}
