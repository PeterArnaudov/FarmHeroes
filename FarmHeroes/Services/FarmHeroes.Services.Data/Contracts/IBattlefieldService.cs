namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;

    public interface IBattlefieldService
    {
        Task StartPatrol();

        Task<int> Collect();

        Task<Hero[]> GetOpponents();

        Task<TViewModel> GetOpponentsViewModel<TViewModel>();
    }
}
