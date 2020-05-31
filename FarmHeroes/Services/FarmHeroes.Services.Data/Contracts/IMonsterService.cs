namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Services.Models.Monsters;
    using FarmHeroes.Web.ViewModels.MonsterModels;

    public interface IMonsterService
    {
        Task<Monster> GetMonsterByLevel(int level);

        Task<FightMonster> GenerateFightMonster(Monster databaseMonster);

        Task<Monster[]> GetAllMonsters();

        Task AddMonster(MonsterInputModel inputModel);

        Task<MonsterInputModel> GetMonsterInputModelById(int id);

        Task EditMonster(MonsterInputModel inputModel);

        Task DeleteMonster(int id);
    }
}
