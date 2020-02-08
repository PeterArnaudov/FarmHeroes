namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;

    public class HealthService : IHealthService
    {
        private readonly IMapper mapper;
        private readonly IHeroService heroService;
        private readonly FarmHeroesDbContext context;

        public HealthService(IMapper mapper, IHeroService heroService, FarmHeroesDbContext context)
        {
            this.mapper = mapper;
            this.heroService = heroService;
            this.context = context;
        }

        public async Task<Health> GetCurrentHeroHealth()
        {
            Hero hero = await this.heroService.GetCurrentHero();
            Health health = hero.Health;

            return health;
        }

        public async Task<Health> GetHealthById(int id)
        {
            Health health = await this.context.Healths.FindAsync(id);

            return health;
        }

        public async Task<TViewModel> GetCurrentHeroHealthViewModel<TViewModel>()
        {
            Health health = await this.GetCurrentHeroHealth();

            TViewModel viewModel = this.mapper.Map<TViewModel>(health);

            return viewModel;
        }

        public async Task IncreaseMaximumHealth(int mass)
        {
            Health health = await this.GetCurrentHeroHealth();
            health.Maximum = HealthFormulas.CalculateMaximumHealth(mass);

            await this.context.SaveChangesAsync();
        }

        public async Task ReduceHealthById(int id, int damage)
        {
            Health health = await this.GetHealthById(id);
            health.Current -= damage;

            if (health.Current <= 0)
            {
                health.Current = 1;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfDead(int id)
        {
            Health health = await this.GetHealthById(id);

            return health.Current == 1;
        }
    }
}
