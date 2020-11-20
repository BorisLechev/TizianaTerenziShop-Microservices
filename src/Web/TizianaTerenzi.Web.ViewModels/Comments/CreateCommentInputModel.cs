namespace TizianaTerenzi.Web.ViewModels.Comments
{
    public class CreateCommentInputModel
    {
        public int ProductId { get; set; } // hidden field

        public int ParentId { get; set; }

        public string Content { get; set; }
    }
}
