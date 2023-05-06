namespace TizianaTerenzi.Web.ViewModels.Votes
{
    public class ProductVoteResponseModel
    {
        public int ProductId { get; set; }

        public int NumberOfVoters { get; set; }

        public double AverageVote { get; set; }

        public double ShareOfVotesWithValueOfFive { get; set; }

        public double ShareOfVotesWithValueOfFour { get; set; }

        public double ShareOfVotesWithValueOfThree { get; set; }

        public double ShareOfVotesWithValueOfTwo { get; set; }

        public double ShareOfVotesWithValueOfOne { get; set; }
    }
}
