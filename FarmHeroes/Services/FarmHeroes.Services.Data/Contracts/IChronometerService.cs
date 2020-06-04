namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.ChronometerModels;

    public interface IChronometerService
    {
        Task<Chronometer> GetChronometer(int id = 0);

        Task<TViewModel> GetCurrentHeroChronometerViewModel<TViewModel>();

        Task SetWorkUntil(int seconds, WorkStatus workStatus);

        Task NullifyWorkUntil();

        Task SetCannotAttackHeroUntil(int id, int seconds);

        Task SetCannotAttackMonsterUntil(int id, int seconds);

        Task SetCannotBeAttackedUntil(int id, int seconds);

        Task UpdateChronometer(ChronometerModifyInputModel inputModel);
    }
}
