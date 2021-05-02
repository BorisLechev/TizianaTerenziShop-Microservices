namespace TizianaTerenzi.Web.ViewModels.Votes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PostProductVoteInputModel
    {
        public int ProductId { get; set; }

        [Range(1, 5, ErrorMessage = "Vote should be between {1} and {2}.")]
        public byte Value { get; set; }
    }
}
