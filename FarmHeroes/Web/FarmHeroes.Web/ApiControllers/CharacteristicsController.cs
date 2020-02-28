namespace FarmHeroes.Web.ApiControllers
{
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class CharacteristicsController : ApiController
    {
        private readonly ICharacteristicsService characteristicsService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IHealthService healthService;

        public CharacteristicsController(ICharacteristicsService characteristicsService, IResourcePouchService resourcePouchService, IHealthService healthService)
        {
            this.characteristicsService = characteristicsService;
            this.resourcePouchService = resourcePouchService;
            this.healthService = healthService;
        }

        [HttpGet("PracticeAttack")]
        public async Task<ActionResult<object>> PracticeAttack()
        {
            try
            {
                int attack = await this.characteristicsService.IncreaseAttack();

                object result = new
                {
                    Stat = attack,
                    Price = CharacteristicsFormulas.CalculateAttackPrice(attack),
                    this.resourcePouchService.GetCurrentHeroResources().Result.Gold,
                };

                return result;
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [HttpGet("PracticeDefense")]
        public async Task<ActionResult<object>> PracticeDefense()
        {
            try
            {
                int defense = await this.characteristicsService.IncreaseDefense();

                object result = new
                {
                    Stat = defense,
                    Price = CharacteristicsFormulas.CalculateDefensePrice(defense),
                    this.resourcePouchService.GetCurrentHeroResources().Result.Gold,
                };

                return result;
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [HttpGet("PracticeMass")]
        public async Task<ActionResult<object>> PracticeMass()
        {
            try
            {
                int mass = await this.characteristicsService.IncreaseMass();
                Health health = await this.healthService.GetCurrentHeroHealth();

                object result = new
                {
                    Stat = mass,
                    Price = CharacteristicsFormulas.CalculateMassPrice(mass),
                    this.resourcePouchService.GetCurrentHeroResources().Result.Gold,
                    health.Current,
                    health.Maximum,
                };

                return result;
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [HttpGet("PracticeMastery")]
        public async Task<ActionResult<object>> PracticeMastery()
        {
            try
            {
                int mastery = await this.characteristicsService.IncreaseMastery();

                object result = new
                {
                    Stat = mastery,
                    Price = CharacteristicsFormulas.CalculateDefensePrice(mastery),
                    this.resourcePouchService.GetCurrentHeroResources().Result.Gold,
                };

                return result;
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}
