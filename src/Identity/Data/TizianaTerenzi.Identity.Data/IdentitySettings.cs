namespace TizianaTerenzi.Identity.Data
{
    public class IdentitySettings
    {
        public string AdminMail { get; private set; } = "admin";

        public string AdminUserName { get; private set; } = "admin";

        public string AdminPassword { get; private set; } = "123456";

        public string RegularUserMail { get; private set; } = "petri@abv.bg";

        public string RegularUserUserName { get; private set; } = "petri";

        public string RegularUserPassword { get; private set; } = "123456";
    }
}
