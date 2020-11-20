namespace TizianaTerenzi.Web.ViewModels.Votes
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class DetailsVotesViewModel : IMapFrom<Vote>
    {
        public string UserId { get; set; }

        public VoteType Type { get; set; }
    }
}
