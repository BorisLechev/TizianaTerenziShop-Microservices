namespace TizianaTerenzi.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Web.ViewModels;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }

        [Route("/about-us")]
        public IActionResult AboutUs()
        {
            return this.View();
        }

        [Route("/privacy")]
        public IActionResult Privacy()
        {
            return this.View();
        }

        [Route("/terms")]
        public IActionResult Terms()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
