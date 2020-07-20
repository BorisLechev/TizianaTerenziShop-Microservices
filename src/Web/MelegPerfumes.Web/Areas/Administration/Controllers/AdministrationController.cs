namespace MelegPerfumes.Web.Areas.Administration.Controllers
{
    using MelegPerfumes.Common;
    using MelegPerfumes.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
