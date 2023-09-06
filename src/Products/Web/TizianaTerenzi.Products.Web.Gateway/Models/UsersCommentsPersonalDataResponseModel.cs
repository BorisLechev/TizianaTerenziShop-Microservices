namespace TizianaTerenzi.Products.Web.Gateway.Models
{
    public class UsersCommentsPersonalDataResponseModel
    {
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<UsersCommentVotesPersonalDataResponseModel> Votes { get; set; }
    }
}
