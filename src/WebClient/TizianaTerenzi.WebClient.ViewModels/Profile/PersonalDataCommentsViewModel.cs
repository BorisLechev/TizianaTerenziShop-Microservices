namespace TizianaTerenzi.WebClient.ViewModels.Profile
{
    using System;
    using System.Collections.Generic;

    public class PersonalDataCommentsViewModel
    {
        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public ICollection<PersonalDataCommentVoteViewModel> Votes { get; set; }
    }
}
