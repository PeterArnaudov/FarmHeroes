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
            int attack = await this.characteristicsService.IncreaseAttack();

            object result = new
            {
                Stat = attack,
                Price = CharacteristicsFormulas.CalculateAttackPrice(attack),
                this.resourcePouchService.GetResourcePouch().Result.Gold,
            };

            return result;
        }

        [HttpGet("PracticeDefense")]
        public async Task<ActionResult<object>> PracticeDefense()
        {
            int defense = await this.characteristicsService.IncreaseDefense();

            object result = new
            {
                Stat = defense,
                Price = CharacteristicsFormulas.CalculateDefensePrice(defense),
                this.resourcePouchService.GetResourcePouch().Result.Gold,
            };

            return result;
        }

        [HttpGet("PracticeMass")]
        public async Task<ActionResult<object>> PracticeMass()
        {
            int mass = await this.characteristicsService.IncreaseMass();
            Health health = await this.healthService.GetHealth();

            object result = new
            {
                Stat = mass,
                Price = CharacteristicsFormulas.CalculateMassPrice(mass),
                this.resourcePouchService.GetResourcePouch().Result.Gold,
                health.Current,
                health.Maximum,
            };

            return result;
        }

        [HttpGet("PracticeMastery")]
        public async Task<ActionResult<object>> PracticeMastery()
        {
            int mastery = await this.characteristicsService.IncreaseMastery();

            object result = new
            {
                Stat = mastery,
                Price = CharacteristicsFormulas.CalculateDefensePrice(mastery),
                this.resourcePouchService.GetResourcePouch().Result.Gold,
            };

            return result;
        }
    }
}
