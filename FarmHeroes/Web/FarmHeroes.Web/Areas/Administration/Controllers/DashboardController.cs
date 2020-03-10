namespace FarmHeroes.Web.Areas.Administration.Controllers
{
    using FarmHeroes.Services.Data;
    using FarmHeroes.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        public DashboardController()
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
