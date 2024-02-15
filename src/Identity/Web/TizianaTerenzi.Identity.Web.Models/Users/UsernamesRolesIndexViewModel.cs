namespace TizianaTerenzi.Identity.Web.Models.Users
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UsernamesRolesIndexViewModel
    {
        public string RoleId { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public string UserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
