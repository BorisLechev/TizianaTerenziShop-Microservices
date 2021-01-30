namespace TizianaTerenzi.Web.ViewModels.Votes
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class DetailsCommentVotesViewModel : IMapFrom<CommentVote>
    {
        public string UserId { get; set; }

        public CommentVoteType Type { get; set; }
    }
}
