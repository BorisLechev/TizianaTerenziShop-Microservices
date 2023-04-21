namespace TizianaTerenzi.Products.Web.Models.Votes
{
    public class GroupProductVoteValuesViewModel<TOne, TTwo, TThree, TFour, TFive, TSix, TSeven, TEight>
    {
        public TOne Group { get; set; }

        public TTwo GroupVotesWithValue5 { get; set; }

        public TThree GroupVotesWithValue4 { get; set; }

        public TFour GroupVotesWithValue3 { get; set; }

        public TFive GroupVotesWithValue2 { get; set; }

        public TSix GroupVotesWithValue1 { get; set; }

        public TSeven CountOfVotes { get; set; }

        public TEight AverageVotes { get; set; }
    }
}
