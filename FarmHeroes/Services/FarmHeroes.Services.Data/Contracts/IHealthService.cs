namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.HealthModels;

    public interface IHealthService
    {
        Task<Health> GetCurrentHeroHealth();

        Task<TViewModel> GetCurrentHeroHealthViewModel<TViewModel>();

        Task HealCurrentHero(int amount, int gold);

        Task HealCurrentHeroToMaximum(int crystals);

        Task<Health> GetHealthById(int id);

        Task IncreaseMaximumHealth(int mass);

        Task ReduceHealthById(int id, int damage);

        Task<bool> CheckIfDead(int id);

        Task UpdateHealth(HealthModifyInputModel inputModel);
    }
}
