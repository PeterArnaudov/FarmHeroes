namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.FightModels;

    public interface IFightService
    {
        Task<int> InitiateFight(int opponentId);

        Task<int> InitiateMonsterFight(int monsterLevel = 0);

        Task<Fight> GetFight(int id);

        Task<TViewModel> GetFightViewModel<TViewModel>(int id);
    }
}
