namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Services.Data.Models.Monsters;

    public interface IMonsterService
    {
        Task<Monster> GetMonsterByLevel(int level);

        Task<FightMonster> GenerateFightMonster(Monster databaseMonster);
    }
}
