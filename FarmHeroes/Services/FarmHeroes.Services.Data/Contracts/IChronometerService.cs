namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.ChronometerModels;

    public interface IChronometerService
    {
        Task<Chronometer> GetCurrentHeroChronometer();

        Task<TViewModel> GetCurrentHeroChronometerViewModel<TViewModel>();

        Task SetWorkUntil(int seconds, WorkStatus workStatus);

        Task NullifyWorkUntil();

        Task SetCannotAttackHeroUntilById(int id, int seconds);

        Task SetCannotAttackMonsterUntilById(int id, int seconds);

        Task SetCannotBeAttackedUntilById(int id, int seconds);

        Task UpdateChronometer(ChronometerModifyInputModel inputModel);
    }
}
