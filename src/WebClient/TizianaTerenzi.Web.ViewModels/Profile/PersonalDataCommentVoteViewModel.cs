namespace TizianaTerenzi.Web.ViewModels.Profile
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class PersonalDataCommentVoteViewModel : IMapFrom<CommentVote>
    {
        public DateTime CreatedOn { get; set; }

        public CommentVoteType Type { get; set; }
    }
}
