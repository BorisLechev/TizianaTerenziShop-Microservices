namespace TizianaTerenzi.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Web.ViewModels.Subscribe;

    public class SubscribeNewsletterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var viewModel = new SubscribeInputModel
            {
                Email = null,
            };

            return this.View(viewModel);
        }
    }
}
