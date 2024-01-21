namespace TizianaTerenzi.Common.Messages.Identity
{
    public class UserAddedInAdminStatisticsMessage
    {
        public string UserId { get; set; }

        public string RoleName { get; set; }

        public bool IsBlocked { get; set; }
    }
}
