namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.HealthModels;

    public class HealthService : IHealthService
    {
        private readonly IMapper mapper;
        private readonly IHeroService heroService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly FarmHeroesDbContext context;

        public HealthService(IMapper mapper, IHeroService heroService, IResourcePouchService resourcePouchService, FarmHeroesDbContext context)
        {
            this.mapper = mapper;
            this.heroService = heroService;
            this.resourcePouchService = resourcePouchService;
            this.context = context;
        }

        public async Task<Health> GetHealth(int id = 0)
        {
            Hero hero = await this.heroService.GetHero(id);
            Health health = hero.Health;

            return health;
        }

        public async Task<TViewModel> GetCurrentHeroHealthViewModel<TViewModel>()
        {
            Health health = await this.GetHealth();

            TViewModel viewModel = this.mapper.Map<TViewModel>(health);

            return viewModel;
        }

        public async Task IncreaseMaximumHealth(int mass)
        {
            Health health = await this.GetHealth();
            health.Maximum = HealthFormulas.CalculateMaximumHealth(mass);

            await this.context.SaveChangesAsync();
        }

        public async Task HealCurrentHero(int amount, int gold)
        {
            Health health = await this.GetHealth();

            await this.resourcePouchService.DecreaseResource(ResourceNames.Gold, gold);
            health.Current += amount;

            if (health.Current > health.Maximum)
            {
                health.Current = health.Maximum;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task HealCurrentHeroToMaximum(int crystals)
        {
            Health health = await this.GetHealth();

            await this.resourcePouchService.DecreaseResource(ResourceNames.Crystals, crystals);
            health.Current = health.Maximum;

            await this.context.SaveChangesAsync();
        }

        public async Task ReduceHealth(int damage, int id = 0)
        {
            Health health = await this.GetHealth(id);
            health.Current -= damage;

            if (health.Current <= 0)
            {
                health.Current = 1;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfDead(int id = 0)
        {
            Health health = await this.GetHealth(id);

            return health.Current == 1;
        }

        public async Task UpdateHealth(HealthModifyInputModel inputModel)
        {
            Hero hero = await this.heroService.GetHeroByName(inputModel.Name);

            hero.Health.Current = inputModel.HealthCurrent;
            hero.Health.Maximum = inputModel.HealthMaximum;

            await this.context.SaveChangesAsync();
        }
    }
}
