namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
