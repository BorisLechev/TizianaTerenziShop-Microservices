namespace TizianaTerenzi.Products.Web.Gateway.Models
{
    public class DownloadPersonalDataResponseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public string Town { get; set; }

        public string PostalCode { get; set; }

        public string CountryName { get; set; }

        public string Address { get; set; }

        public IEnumerable<ProductsFromUsersWishlistPersonalDataResponseModel> FavoriteProducts { get; set; }

        public IEnumerable<UsersProductVotesPersonalDataResponseModel> ProductVotes { get; set; }

        public IEnumerable<UsersCommentsPersonalDataResponseModel> Comments { get; set; }

        //public ICollection<PersonalDataOrdersViewModel> Orders { get; set; }

        //public ICollection<PersonalDataChatUserGroupViewModel> ChatUserGroups { get; set; }
    }
}
