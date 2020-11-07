namespace MelegPerfumes.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Ganss.XSS;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;
    using MelegPerfumes.Web.ViewModels.Votes;

    public class ProductCommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content); // zaradi TinyMC

        public string UserUserName { get; set; } // Comment->ApplicationUser->IdentityUser->UserName

        public string UserId { get; set; }

        public int VotesSum { get; set; }

        public IEnumerable<DetailsVotesViewModel> Votes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, ProductCommentViewModel>()
                .ForMember(dest => dest.VotesSum, opt => opt.MapFrom(src => src.Votes.Sum(v => (int)v.Type)));
        }
    }
}
