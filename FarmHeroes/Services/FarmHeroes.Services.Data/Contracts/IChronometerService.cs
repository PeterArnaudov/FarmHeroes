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

        Task SetCannotAttackHeroUntil(int seconds, int id = 0);

        Task SetCannotAttackMonsterUntil(int seconds, int id = 0);

        Task SetCannotBeAttackedUntil(int seconds, int id = 0);

        Task SetCannotDungeonUntil(int seconds, int id = 0);

        Task SetSailingUntil(int seconds, bool setToNull = false, int id = 0);

        Task UpdateChronometer(ChronometerModifyInputModel inputModel);
    }
}
