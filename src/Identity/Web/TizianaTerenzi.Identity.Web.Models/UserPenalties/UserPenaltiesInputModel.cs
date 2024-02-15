namespace TizianaTerenzi.Identity.Web.Models.UserPenalties
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UserPenaltiesInputModel
    {
        public string UserId { get; set; }

        public string ReasonToBeBlocked { get; set; }

        public IEnumerable<SelectListItem> BlockedUsernames { get; set; }

        public IEnumerable<SelectListItem> UnblockedUsernames { get; set; }
    }
}
