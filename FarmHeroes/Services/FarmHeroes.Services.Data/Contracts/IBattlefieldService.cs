namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;

    public interface IBattlefieldService
    {
        Task<int> StartPatrol();

        Task<CollectedResourcesViewModel> Collect();

        Task<Hero[]> GetOpponents();

        Task<TViewModel> GetOpponentsViewModel<TViewModel>();
    }
}
