namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;

    public class CharacteristicsService : ICharacteristicsService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;
        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcesService;
        private readonly IHealthService healthService;

        public CharacteristicsService(FarmHeroesDbContext context, IMapper mapper, IHeroService heroService, IResourcePouchService resourcePouchService, IHealthService healthService)
        {
            this.context = context;
            this.mapper = mapper;
            this.heroService = heroService;
            this.resourcesService = resourcePouchService;
            this.healthService = healthService;
        }

        public async Task<Characteristics> GetHeroCharacteristicsByIdAsync(int id)
        {
            Characteristics characteristics = await this.context.Characteristics.FindAsync(id);
            return characteristics;
        }

        public async Task<Characteristics> GetCurrentHeroCharacteristicsAsync()
        {
            Hero hero = await this.heroService.GetCurrentHero();
            Characteristics characteristics = hero.Characteristics;

            return characteristics;
        }

        public async Task<TViewModel> GetCurrentHeroCharacteristicsViewModelAsync<TViewModel>()
        {
            Hero hero = await this.heroService.GetCurrentHero();
            Characteristics characteristics = hero.Characteristics;

            TViewModel viewModel = this.mapper.Map<TViewModel>(characteristics);

            return viewModel;
        }

        public async Task IncreaseAttack()
        {
            Characteristics characteristics = await this.GetCurrentHeroCharacteristicsAsync();
            int goldNeeded = CharacteristicsFormulas.CalculateAttackPrice(characteristics.Attack);

            await this.resourcesService.DecreaseCurrentHeroGold(goldNeeded);
            characteristics.Attack++;

            await this.context.SaveChangesAsync();
        }

        public async Task IncreaseDefense()
        {
            Characteristics characteristics = await this.GetCurrentHeroCharacteristicsAsync();
            int goldNeeded = CharacteristicsFormulas.CalculateDefensePrice(characteristics.Defense);

            await this.resourcesService.DecreaseCurrentHeroGold(goldNeeded);
            characteristics.Defense++;

            await this.context.SaveChangesAsync();
        }

        public async Task IncreaseMass()
        {
            Characteristics characteristics = await this.GetCurrentHeroCharacteristicsAsync();
            int goldNeeded = CharacteristicsFormulas.CalculateMassPrice(characteristics.Mass);

            await this.resourcesService.DecreaseCurrentHeroGold(goldNeeded);
            characteristics.Mass++;

            await this.healthService.IncreaseMaximumHealth(characteristics.Mass);

            await this.context.SaveChangesAsync();
        }

        public async Task IncreaseMastery()
        {
            Characteristics characteristics = await this.GetCurrentHeroCharacteristicsAsync();
            int goldNeeded = CharacteristicsFormulas.CalculateMasteryPrice(characteristics.Mastery);

            await this.resourcesService.DecreaseCurrentHeroGold(goldNeeded);
            characteristics.Mastery++;

            await this.context.SaveChangesAsync();
        }
    }
}
