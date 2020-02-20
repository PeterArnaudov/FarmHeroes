namespace FarmHeroes.Services.Data
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Services.Data.Models.Monsters;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class MonsterService : IMonsterService
    {
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
            fightMonster.Level = databaseMonster.Level;
            fightMonster.Characteristics = this.GenerateFightMonsterCharacteristics(databaseMonster.StatPercentage, hero.Characteristics);
            fightMonster.Health = hero.Health.Maximum;

            return fightMonster;
        }

        private Characteristics GenerateFightMonsterCharacteristics(int monsterBattlePowerPercent, Characteristics heroCharacteristics)
        {
            Characteristics monsterCharacteristics = new Characteristics();

            int heroBattlePower = CharacteristicsFormulas.CalculateBattlePower(heroCharacteristics);

            Random random = new Random();

            int maxPercent = 100;
            int monsterAttackPercent = random.Next(25, 50);
            int monsterDefensePercent = random.Next(10, 40);
            int monsterMasteryPercent = maxPercent - monsterAttackPercent - monsterDefensePercent;
            int monsterBattlePower = MonsterFormulas.CalculateBattlePower(heroBattlePower, monsterBattlePowerPercent);

            monsterCharacteristics.Attack = MonsterFormulas.CalculateAttack(monsterBattlePower, monsterAttackPercent);
            monsterCharacteristics.Defense = MonsterFormulas.CalculateDefense(monsterBattlePower, monsterAttackPercent);
            monsterCharacteristics.Mastery = MonsterFormulas.CalculateMastery(monsterBattlePower, monsterMasteryPercent);
            monsterCharacteristics.Mass = heroCharacteristics.Mass;

            return monsterCharacteristics;
        }
    }
}
