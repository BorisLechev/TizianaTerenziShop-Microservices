namespace TizianaTerenzi.WebClient.ViewModels.Products
{
    using System;
    using System.Collections.Generic;

    using Ganss.Xss;
    using TizianaTerenzi.WebClient.ViewModels.Votes;

    public class ProductCommentViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content); // zaradi TinyMC

        public string UserId { get; set; }

        public string UserUserName { get; set; } // Comment->ApplicationUser->IdentityUser->UserName

        public int VotesSum { get; set; }

        public IEnumerable<CommentVotesViewModel> Votes { get; set; }
    }
}
