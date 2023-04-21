namespace TizianaTerenzi.Products.Web.Models.Products
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using Ganss.Xss;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Products.Data.Models;
    using TizianaTerenzi.Products.Web.Models.Votes;

    public class ProductCommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
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

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, ProductCommentViewModel>()
                .ForMember(dest => dest.VotesSum, opt => opt.MapFrom(src => src.Votes.Sum(v => (int)v.Type)));
        }
    }
}
