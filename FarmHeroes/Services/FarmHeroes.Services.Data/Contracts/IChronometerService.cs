namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;

    public interface IChronometerService
    {
        Task<Chronometer> GetCurrentHeroChronometer();

        Task<TViewModel> GetCurrentHeroChronometerViewModel<TViewModel>();

        Task SetWorkUntil(int minutes, WorkStatus workStatus);

        Task NullifyWorkUntil();

        Task SetCannotAttackHeroUntilById(int id, int minutes);

        Task SetCannotAttackMonsterUntilById(int id, int minutes);

        Task SetCannotBeAttackedUntilById(int id, int minutes);
    }
}
