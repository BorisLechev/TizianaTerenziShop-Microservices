namespace TizianaTerenzi.Products.Web.Models.Comments
{
    using TizianaTerenzi.Common.Enumerators;

    public class UsersCommentVotesPersonalDataResponseModel
    {
        public CommentVoteType Type { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
