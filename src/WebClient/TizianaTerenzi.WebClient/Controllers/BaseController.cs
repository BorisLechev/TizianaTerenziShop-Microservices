namespace TizianaTerenzi.WebClient.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;

    public abstract class BaseController : Controller
    {
        protected void Error(string message)
        {
            this.TempData[GlobalConstants.TempDataErrorMessageKey] = message;
        }

        protected void Success(string message)
        {
            this.TempData[GlobalConstants.TempDataSuccessMessageKey] = message;
        }
    }
}
