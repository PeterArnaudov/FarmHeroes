namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Web.Filters;
    using Microsoft.AspNetCore.Mvc;

    public class VillageController : Controller
    {
        [Route("/Village")]
        public IActionResult Village()
        {
            return this.View();
        }
    }
}