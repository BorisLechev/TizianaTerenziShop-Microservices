using Ganss.XSS;
using MelegPerfumes.Data.Models;
using MelegPerfumes.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace MelegPerfumes.Web.ViewModels.Products
{
    public class ProductCommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content); // zaradi TinyMC

        // TODO: Change Issuer to User
        public string IssuerUserName { get; set; } // Comment->ApplicationUser->IdentityUser->UserName
    }
}
