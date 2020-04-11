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
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Hero");
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }
    }
}
