namespace FarmHeroes.Web.Controllers
{
    using System.Diagnostics;

    using FarmHeroes.Web.ViewModels;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionDetails = this.HttpContext.Features.Get<IExceptionHandlerFeature>();

            return this.View(new ErrorViewModel { ExceptionMessage = exceptionDetails.Error.Message });
        }
    }
}
