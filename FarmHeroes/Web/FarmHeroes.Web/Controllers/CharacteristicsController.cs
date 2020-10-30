namespace FarmHeroes.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CharacteristicsController : BaseController
    {
        private readonly ICharacteristicsService characteristicsService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IHealthService healthService;

        public CharacteristicsController(
            ICharacteristicsService characteristicsService,
            IResourcePouchService resourcePouchService,
            IHealthService healthService)
        {
            this.characteristicsService = characteristicsService;
            this.resourcePouchService = resourcePouchService;
            this.healthService = healthService;
        }

        public async Task<IActionResult> Index()
        {
            CharacteristicsPracticeViewModel viewModel =
                await this.characteristicsService.GetCurrentHeroCharacteristicsViewModel<CharacteristicsPracticeViewModel>();

            return this.View(viewModel);
        }

        public async Task<ActionResult<object>> PracticeAttack()
        {
            int attack = await this.characteristicsService.IncreaseAttack();

            object result = new
            {
                Stat = attack,
                Price = CharacteristicsFormulas.CalculateAttackPrice(attack),
                Gold = await this.resourcePouchService.GetResource(ResourceNames.Gold),
            };

            return result;
        }

        public async Task<ActionResult<object>> PracticeDefense()
        {
            int defense = await this.characteristicsService.IncreaseDefense();

            object result = new
            {
                Stat = defense,
                Price = CharacteristicsFormulas.CalculateDefensePrice(defense),
                Gold = await this.resourcePouchService.GetResource(ResourceNames.Gold),
            };

            return result;
        }

        public async Task<ActionResult<object>> PracticeMass()
        {
            int mass = await this.characteristicsService.IncreaseMass();
            Health health = await this.healthService.GetHealth();

            object result = new
            {
                Stat = mass,
                Price = CharacteristicsFormulas.CalculateMassPrice(mass),
                Gold = await this.resourcePouchService.GetResource(ResourceNames.Gold),
                health.Current,
                health.Maximum,
            };

            return result;
        }

        public async Task<ActionResult<object>> PracticeMastery()
        {
            int mastery = await this.characteristicsService.IncreaseMastery();

            object result = new
            {
                Stat = mastery,
                Price = CharacteristicsFormulas.CalculateDefensePrice(mastery),
                Gold = await this.resourcePouchService.GetResource(ResourceNames.Gold),
            };

            return result;
        }

        public async Task<ActionResult<object>> PracticeDexterity()
        {
            int mastery = await this.characteristicsService.IncreaseDexterity();

            object result = new
            {
                Stat = mastery,
                Price = CharacteristicsFormulas.CalculateDexterityPrice(mastery),
                Gold = await this.resourcePouchService.GetResource(ResourceNames.Gold),
            };

            return result;
        }
    }
}
