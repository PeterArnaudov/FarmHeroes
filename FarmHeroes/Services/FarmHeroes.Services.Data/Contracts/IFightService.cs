namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.FightModels;

    public interface IFightService
    {
        Task<int> InitiateFight(int opponentId);

        Task<int> InitiateMonsterFight(int? monsterLevel);

        Task<Fight> GetFightById(int id);

        Task<TViewModel> GetFightViewModelById<TViewModel>(int id);
    }
}
