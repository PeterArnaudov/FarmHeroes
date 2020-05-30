namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Services.Models.Monsters;

    using Microsoft.EntityFrameworkCore;

    public class MonsterService : IMonsterService
    {
        private const int MaxPercent = 100;
        private const int MinimumPercentAttack = 25;
        private const int MaximumPercentAttack = 50;
        private const int MinimumPercentDefense = 10;
        private const int MaximumPercentDefense = 40;

        private readonly IHeroService heroService;
        private readonly FarmHeroesDbContext context;

        public MonsterService(IHeroService heroService, FarmHeroesDbContext context)
        {
            this.heroService = heroService;
            this.context = context;
        }

        public async Task<Monster> GetMonsterByLevel(int level)
        {
            return await this.context.Monsters.SingleOrDefaultAsync(m => m.Level == level);
        }

        public async Task<FightMonster> GenerateFightMonster(Monster databaseMonster)
        {
            FightMonster fightMonster = new FightMonster();

            Hero hero = await this.heroService.GetCurrentHero();

            fightMonster.Name = databaseMonster.Name;
            fightMonster.AvatarUrl = databaseMonster.AvatarUrl;
            fightMonster.Level = databaseMonster.Level;
            fightMonster.Characteristics = this.GenerateFightMonsterCharacteristics(databaseMonster.Level, databaseMonster.StatPercentage, hero.Characteristics);
            fightMonster.Health = hero.Health.Maximum;

            return fightMonster;
        }

        private Characteristics GenerateFightMonsterCharacteristics(int monsterLevel, int monsterBattlePowerPercent, Characteristics heroCharacteristics)
        {
            Characteristics monsterCharacteristics = new Characteristics();

            int heroBattlePower = CharacteristicsFormulas.CalculateBattlePower(heroCharacteristics);

            Random random = new Random();

            int maxPercent = MaxPercent;
            int monsterAttackPercent = random.Next(MinimumPercentAttack, MaximumPercentAttack);
            int monsterDefensePercent = random.Next(MinimumPercentDefense, MaximumPercentDefense);
            int monsterMasteryPercent = maxPercent - monsterAttackPercent - monsterDefensePercent;
            int monsterBattlePower = MonsterFormulas.CalculateBattlePower(heroBattlePower, monsterBattlePowerPercent);

            monsterCharacteristics.Attack = MonsterFormulas.CalculateAttack(monsterLevel, monsterBattlePower, monsterAttackPercent);
            monsterCharacteristics.Defense = MonsterFormulas.CalculateDefense(monsterLevel, monsterBattlePower, monsterAttackPercent);
            monsterCharacteristics.Mastery = MonsterFormulas.CalculateMastery(monsterLevel, monsterBattlePower, monsterMasteryPercent);
            monsterCharacteristics.Mass = heroCharacteristics.Mass;

            return monsterCharacteristics;
        }
    }
}
