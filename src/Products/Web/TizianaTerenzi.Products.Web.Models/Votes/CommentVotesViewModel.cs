namespace TizianaTerenzi.Products.Web.Models.Votes
{
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Products.Data.Models;

    public class CommentVotesViewModel : IMapFrom<CommentVote>
    {
        public string UserId { get; set; }

        public CommentVoteType Type { get; set; }
    }
}
