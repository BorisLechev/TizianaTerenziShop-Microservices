namespace TizianaTerenzi.Identity.Web.Models.Identity
{
    public class UserResponseModel
    {
        public UserResponseModel(string token)
        {
            this.Token = token;
        }

        public string Token { get; }
    }
}
