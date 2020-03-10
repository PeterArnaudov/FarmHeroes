namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.HeroModels;

    public interface IHeroService
    {
        Task CreateHero(HeroCreateInputModel inputModel);

        Task<Hero> GetHeroById(int id);

        Task<Hero> GetHeroByName(string name);

        Task<Hero> GetCurrentHero();

        Task<TViewModel> GetCurrentHeroViewModel<TViewModel>();

        Task<TViewModel> GetHeroViewModelById<TViewModel>(int id);

        Task<TViewModel> GetHeroViewModelByName<TViewModel>(string name);

        Task<bool> ValidateCurrentHeroLocation(WorkStatus workStatus);

        Task UpdateBasicInfo(HeroModifyBasicInfoInputModel inputModel);
    }
}
