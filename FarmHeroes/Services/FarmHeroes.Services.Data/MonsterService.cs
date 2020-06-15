namespace FarmHeroes.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Services.Models.Monsters;
    using FarmHeroes.Web.ViewModels.MonsterModels;
    using Microsoft.EntityFrameworkCore;

    public class MonsterService : IMonsterService
    {
        private const int MaxPercent = 100;
        private const int MinimumPercentAttack = 25;
        private const int MaximumPercentAttack = 35;
        private const int MinimumPercentDefense = 15;
        private const int MaximumPercentDefense = 25;
        private const int MinimumPercentMastery = 10;
        private const int MaximumPercentMastery = 20;

        private readonly IHeroService heroService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public MonsterService(IHeroService heroService, FarmHeroesDbContext context, IMapper mapper)
        {
            this.heroService = heroService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Monster> GetMonsterByLevel(int level)
        {
            Monster monster = await this.context.Monsters.SingleOrDefaultAsync(m => m.Level == level);

            if (monster == null)
            {
                monster = await this.context.Monsters.OrderByDescending(x => x.Level).FirstOrDefaultAsync();
                monster.Level = level * 3;
                monster.MinimalRewardModifier += level * 6;
                monster.MaximalRewardModifier += level * 12;
                monster.StatPercentage += level;
                this.context.Entry(monster).State = EntityState.Detached;
            }

            return monster;
        }

        public async Task<FightMonster> GenerateFightMonster(Monster databaseMonster)
        {
            FightMonster fightMonster = new FightMonster();

            Hero hero = await this.heroService.GetHero();

            fightMonster.Name = databaseMonster.Name;
            fightMonster.AvatarUrl = databaseMonster.AvatarUrl;
            fightMonster.Level = databaseMonster.Level;
            fightMonster.Characteristics = this.GenerateFightMonsterCharacteristics(databaseMonster.Level, databaseMonster.StatPercentage, hero.Characteristics);
            fightMonster.Health = hero.Health.Maximum;

            return fightMonster;
        }

        public async Task<Monster[]> GetAllMonsters()
        {
            return await this.context.Monsters.ToArrayAsync();
        }

        public async Task AddMonster(MonsterInputModel inputModel)
        {
            Monster monster = this.mapper.Map<Monster>(inputModel);

            await this.context.Monsters.AddAsync(monster);
            await this.context.SaveChangesAsync();
        }

        public async Task<MonsterInputModel> GetMonsterInputModelById(int id)
        {
            Monster monster = await this.context.Monsters.FindAsync(id);

            return this.mapper.Map<MonsterInputModel>(monster);
        }

        public async Task EditMonster(MonsterInputModel inputModel)
        {
            Monster monster = this.mapper.Map<Monster>(inputModel);

            this.context.Monsters.Update(monster);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteMonster(int id)
        {
            Monster monster = await this.context.Monsters.FindAsync(id);

            this.context.Monsters.Remove(monster);
            await this.context.SaveChangesAsync();
        }

        private Characteristics GenerateFightMonsterCharacteristics(int monsterLevel, int monsterBattlePowerPercent, Characteristics heroCharacteristics)
        {
            Characteristics monsterCharacteristics = new Characteristics();

            int heroBattlePower = CharacteristicsFormulas.CalculateBattlePower(heroCharacteristics);

            Random random = new Random();

            int maxPercent = MaxPercent;
            int monsterAttackPercent = random.Next(MinimumPercentAttack, MaximumPercentAttack);
            int monsterDefensePercent = random.Next(MinimumPercentDefense, MaximumPercentDefense);
            int monsterMasteryPercent = random.Next(MinimumPercentMastery, MaximumPercentMastery);
            int monsterDexterityPercent = maxPercent - monsterAttackPercent - monsterDefensePercent - monsterMasteryPercent;
            int monsterBattlePower = MonsterFormulas.CalculateBattlePower(heroBattlePower, monsterBattlePowerPercent);

            monsterCharacteristics.Attack = MonsterFormulas.CalculateAttack(monsterLevel, monsterBattlePower, monsterAttackPercent);
            monsterCharacteristics.Defense = MonsterFormulas.CalculateDefense(monsterLevel, monsterBattlePower, monsterAttackPercent);
            monsterCharacteristics.Mastery = MonsterFormulas.CalculateMastery(monsterLevel, monsterBattlePower, monsterMasteryPercent);
            monsterCharacteristics.Dexterity = MonsterFormulas.CalculateDexterity(monsterLevel, monsterBattlePower, monsterDexterityPercent);
            monsterCharacteristics.Mass = heroCharacteristics.Mass;

            return monsterCharacteristics;
        }
    }
}
