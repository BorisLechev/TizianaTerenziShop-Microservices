namespace TizianaTerenzi.Identity.Web.Models.Profile
{
    public class DownloadPersonalDataFileResponseModel
    {
        public string UserId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public byte[] File { get; set; }
    }
}
