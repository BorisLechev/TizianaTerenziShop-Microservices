namespace TizianaTerenzi.WebClient.ViewModels.Profile
{
    using System;
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class PersonalDataCommentsViewModel : IMapFrom<Comment>
    {
        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public ICollection<PersonalDataCommentVoteViewModel> Votes { get; set; }
    }
}
