namespace FarmHeroes.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IDungeonService
    {
        Task BuyKey();

        Task StartDungeon();

        Task AttackMonster();

        Task WalkOnFloor();

        Task GoToNextFloor();

        Task EndDungeon(bool voluntarily = true);

        Task<T> GetDungeonViewModel<T>();
    }
}
