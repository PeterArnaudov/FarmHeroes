namespace FarmHeroes.Web.Areas.Administration.Controllers.GameControl
{
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class GameControlController : AdministrationController
    {
        public GameControlController()
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
