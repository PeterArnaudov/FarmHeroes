namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;

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

        public async Task<Characteristics> GetCharacteristics(int id = 0)
        {
            Hero hero = await this.heroService.GetHero(id);

            return hero.Characteristics;
        }

        public async Task<TViewModel> GetCurrentHeroCharacteristicsViewModel<TViewModel>()
        {
            Hero hero = await this.heroService.GetHero();
            Characteristics characteristics = hero.Characteristics;

            TViewModel viewModel = this.mapper.Map<TViewModel>(characteristics);

            return viewModel;
        }

        public async Task<int> IncreaseAttack()
        {
            Characteristics characteristics = await this.GetCharacteristics();
            int goldNeeded = CharacteristicsFormulas.CalculateAttackPrice(characteristics.Attack);

            await this.resourcesService.DecreaseGold(goldNeeded);
            characteristics.Attack++;

            await this.context.SaveChangesAsync();

            return characteristics.Attack;
        }

        public async Task<int> IncreaseDefense()
        {
            Characteristics characteristics = await this.GetCharacteristics();
            int goldNeeded = CharacteristicsFormulas.CalculateDefensePrice(characteristics.Defense);

            await this.resourcesService.DecreaseGold(goldNeeded);
            characteristics.Defense++;

            await this.context.SaveChangesAsync();

            return characteristics.Defense;
        }

        public async Task<int> IncreaseMass()
        {
            Characteristics characteristics = await this.GetCharacteristics();
            int goldNeeded = CharacteristicsFormulas.CalculateMassPrice(characteristics.Mass);

            await this.resourcesService.DecreaseGold(goldNeeded);
            characteristics.Mass++;

            await this.healthService.IncreaseMaximumHealth(characteristics.Mass);

            await this.context.SaveChangesAsync();

            return characteristics.Mass;
        }

        public async Task<int> IncreaseMastery()
        {
            Characteristics characteristics = await this.GetCharacteristics();
            int goldNeeded = CharacteristicsFormulas.CalculateMasteryPrice(characteristics.Mastery);

            await this.resourcesService.DecreaseGold(goldNeeded);
            characteristics.Mastery++;

            await this.context.SaveChangesAsync();

            return characteristics.Mastery;
        }

        public async Task UpdateCharacteristics(CharacteristicsModifyInputModel inputModel)
        {
            Hero hero = await this.heroService.GetHeroByName(inputModel.Name);

            hero.Characteristics.Attack = inputModel.CharacteristicsAttack;
            hero.Characteristics.Defense = inputModel.CharacteristicsDefense;
            hero.Characteristics.Mastery = inputModel.CharacteristicsMastery;
            hero.Characteristics.Mass = inputModel.CharacteristicsMass;

            await this.context.SaveChangesAsync();
        }
    }
}
