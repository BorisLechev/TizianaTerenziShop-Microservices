namespace TizianaTerenzi.Products.Web.Models.Comments
{
    public class UsersCommentsPersonalDataResponseModel
    {
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<UsersCommentVotesPersonalDataResponseModel> Votes { get; set; }
    }
}
