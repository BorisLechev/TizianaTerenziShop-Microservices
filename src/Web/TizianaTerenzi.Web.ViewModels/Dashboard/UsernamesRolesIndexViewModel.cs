namespace TizianaTerenzi.Web.ViewModels.Dashboard
{
    using System.Collections.Generic;

    public class UsernamesRolesIndexViewModel
    {
        public AddUserInRoleInputModel AddUserInRole { get; set; }

        public IEnumerable<string> Usernames { get; set; }
    }
}
