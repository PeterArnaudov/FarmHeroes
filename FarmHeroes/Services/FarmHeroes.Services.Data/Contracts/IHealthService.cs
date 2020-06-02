namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.HealthModels;

    public interface IHealthService
    {
        Task<Health> GetHealth(int id = 0);

        Task<TViewModel> GetCurrentHeroHealthViewModel<TViewModel>();

        Task HealCurrentHero(int amount, int gold);

        Task HealCurrentHeroToMaximum(int crystals);

        Task IncreaseMaximumHealth(int mass);

        Task ReduceHealth(int damage, int id = 0);

        Task<bool> CheckIfDead(int id = 0);

        Task UpdateHealth(HealthModifyInputModel inputModel);
    }
}
