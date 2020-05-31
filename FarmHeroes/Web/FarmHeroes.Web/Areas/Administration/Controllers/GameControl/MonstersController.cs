namespace FarmHeroes.Web.Areas.Administration.Controllers.GameControl
{
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.MonsterModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class MonstersController : AdministrationController
    {
        private readonly IMonsterService monsterService;

        public MonstersController(IMonsterService monsterService)
        {
            this.monsterService = monsterService;
        }

        public async Task<IActionResult> Index()
        {
            Monster[] viewModel = await this.monsterService.GetAllMonsters();

            return this.View(viewModel);
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MonsterInputModel inputModel)
        {
            await this.monsterService.AddMonster(inputModel);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            MonsterInputModel inputModel = await this.monsterService.GetMonsterInputModelById(id);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MonsterInputModel inputModel)
        {
            await this.monsterService.EditMonster(inputModel);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.monsterService.DeleteMonster(id);

            return this.RedirectToAction("Index");
        }
    }
}
