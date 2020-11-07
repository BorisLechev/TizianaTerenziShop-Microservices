namespace MelegPerfumes.Web.ViewModels.Votes
{
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class DetailsVotesViewModel : IMapFrom<Vote>
    {
        public string UserId { get; set; }

        public VoteType Type { get; set; }
    }
}
