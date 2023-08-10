namespace TizianaTerenzi.WebClient.ViewModels.Identity
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
