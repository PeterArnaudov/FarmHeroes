namespace FarmHeroes.Web.Areas.Administration.Controllers
{
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;
    using FarmHeroes.Web.ViewModels.ChronometerModels;
    using FarmHeroes.Web.ViewModels.HealthModels;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using FarmHeroes.Web.ViewModels.LevelModels;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
    using FarmHeroes.Web.ViewModels.UserModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class PlayerControlController : AdministrationController
    {
        private readonly IUserService userService;
        private readonly IHeroService heroService;
        private readonly ILevelService levelService;
        private readonly IHealthService healthService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly ICharacteristicsService characteristicsService;
        private readonly IChronometerService chronometerService;

        public PlayerControlController(IUserService userService, IHeroService heroService, ILevelService levelService, IHealthService healthService, IResourcePouchService resourcePouchService, ICharacteristicsService characteristicsService, IChronometerService chronometerService)
        {
            this.userService = userService;
            this.heroService = heroService;
            this.levelService = levelService;
            this.healthService = healthService;
            this.resourcePouchService = resourcePouchService;
            this.characteristicsService = characteristicsService;
            this.chronometerService = chronometerService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Ban()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Ban(BanInputModel inputModel)
        {
            await this.userService.BanUserByUsername(inputModel);
            return this.RedirectToAction("Ban");
        }

        public IActionResult Roles()
        {
            return this.View();
        }

        [Route("/{area}/{controller}/{action}/{roleName}")]
        public IActionResult ModifyRole(string roleName)
        {
            return this.View("ModifyRole", roleName);
        }

        [HttpPost]
        public async Task<IActionResult> Promote(string username, string roleName)
        {
            await this.userService.AddUserToRole(username, roleName);

            return this.Redirect(this.HttpContext.Request.Headers["Referer"]);
        }

        public async Task<IActionResult> Demote(string username, string roleName)
        {
            await this.userService.RemoveUserFromRole(username, roleName);

            return this.Redirect(this.HttpContext.Request.Headers["Referer"]);
        }

        public IActionResult FindHero()
        {
            return this.View();
        }

        public async Task<IActionResult> ModifyHero(string name)
        {
            await this.userService.CheckIfUserExists(name);

            return this.View("ModifyHero", name);
        }

        public async Task<IActionResult> ModifyBasicInfo(string name)
        {
            await this.userService.CheckIfUserExists(name);

            HeroModifyBasicInfoInputModel inputModel = await this.heroService.GetHeroViewModelByName<HeroModifyBasicInfoInputModel>(name);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyBasicInfo(HeroModifyBasicInfoInputModel inputModel)
        {
            await this.userService.CheckIfUserExists(inputModel.Name);

            await this.heroService.UpdateBasicInfo(inputModel);

            return this.RedirectToAction("ModifyHero", new { inputModel.Name });
        }

        public async Task<IActionResult> ModifyLevel(string name)
        {
            await this.userService.CheckIfUserExists(name);

            LevelModifyInputModel inputModel = await this.heroService.GetHeroViewModelByName<LevelModifyInputModel>(name);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyLevel(LevelModifyInputModel inputModel)
        {
            await this.userService.CheckIfUserExists(inputModel.Name);

            await this.levelService.UpdateLevel(inputModel);

            return this.RedirectToAction("ModifyHero", new { inputModel.Name });
        }

        public async Task<IActionResult> ModifyHealth(string name)
        {
            await this.userService.CheckIfUserExists(name);

            HealthModifyInputModel inputModel = await this.heroService.GetHeroViewModelByName<HealthModifyInputModel>(name);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyHealth(HealthModifyInputModel inputModel)
        {
            await this.userService.CheckIfUserExists(inputModel.Name);

            await this.healthService.UpdateHealth(inputModel);

            return this.RedirectToAction("ModifyHero", new { inputModel.Name });
        }

        public async Task<IActionResult> ModifyResourcePouch(string name)
        {
            await this.userService.CheckIfUserExists(name);

            ResourcePouchModifyInputModel inputModel = await this.heroService.GetHeroViewModelByName<ResourcePouchModifyInputModel>(name);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyResourcePouch(ResourcePouchModifyInputModel inputModel)
        {
            await this.userService.CheckIfUserExists(inputModel.Name);

            await this.resourcePouchService.UpdateResourcePouch(inputModel);

            return this.RedirectToAction("ModifyHero", new { inputModel.Name });
        }

        public async Task<IActionResult> ModifyCharacteristics(string name)
        {
            await this.userService.CheckIfUserExists(name);

            CharacteristicsModifyInputModel inputModel = await this.heroService.GetHeroViewModelByName<CharacteristicsModifyInputModel>(name);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyCharacteristics(CharacteristicsModifyInputModel inputModel)
        {
            await this.userService.CheckIfUserExists(inputModel.Name);

            await this.characteristicsService.UpdateCharacteristics(inputModel);

            return this.RedirectToAction("ModifyHero", new { inputModel.Name });
        }

        public async Task<IActionResult> ModifyChronometer(string name)
        {
            await this.userService.CheckIfUserExists(name);

            ChronometerModifyInputModel inputModel = await this.heroService.GetHeroViewModelByName<ChronometerModifyInputModel>(name);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyChronometer(ChronometerModifyInputModel inputModel)
        {
            await this.userService.CheckIfUserExists(inputModel.Name);

            await this.chronometerService.UpdateChronometer(inputModel);

            return this.RedirectToAction("ModifyHero", new { inputModel.Name });
        }
    }
}
