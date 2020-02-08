namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;

    public class StatisticsService : IStatisticsService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IHeroService heroService;
        private readonly IMapper mapper;

        public StatisticsService(FarmHeroesDbContext context, IHeroService heroService, IMapper mapper)
        {
            this.context = context;
            this.heroService = heroService;
            this.mapper = mapper;
        }

        public async Task<Statistics> GetHeroStatisticsByIdAsync(int id)
        {
            Statistics statistics = await this.context.Statistics.FindAsync(id);
            return statistics;
        }

        public async Task<Statistics> GetCurrentHeroStatistcs()
        {
            Hero hero = await this.heroService.GetCurrentHero();
            Statistics statistics = hero.Statistics;

            return statistics;
        }

        public async Task<TViewModel> GetCurrentHeroStatisticsViewModel<TViewModel>()
        {
            Statistics statistics = await this.GetCurrentHeroStatistcs();
            TViewModel viewModel = this.mapper.Map<TViewModel>(statistics);

            return viewModel;
        }

        public async Task UpdateStatistics(Statistics statistics)
        {
            this.context.Statistics.Update(statistics);
            await this.context.SaveChangesAsync();
        }
    }
}
