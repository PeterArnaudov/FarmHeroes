namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;

    public interface IStatisticsService
    {
        Task<Statistics> GetStatistcs(int id = 0);

        Task<TViewModel> GetCurrentHeroStatisticsViewModel<TViewModel>();

        Task UpdateStatistics(Statistics statistics);
    }
}
