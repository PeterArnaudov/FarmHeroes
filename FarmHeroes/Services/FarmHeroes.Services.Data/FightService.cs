namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MappingModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class FightService : IFightService
    {
        private const int Rounds = 5;
        private const int ExperiencePerWin = 4;
        private const int MinutesUntilNextHeroAttack = 15;
        private const int MinutesDefenseGranted = 60;

        private readonly IHeroService heroService;
        private readonly IHealthService healthService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IChronometerService chronometerService;
        private readonly ILevelService levelService;
        private readonly IStatisticsService statisticsService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public FightService(IHeroService heroService, IHealthService healthService, IResourcePouchService resourcePouchService, IChronometerService chronometerService, ILevelService levelService, IStatisticsService statisticsService, FarmHeroesDbContext context, IMapper mapper)
        {
            this.heroService = heroService;
            this.healthService = healthService;
            this.resourcePouchService = resourcePouchService;
            this.chronometerService = chronometerService;
            this.levelService = levelService;
            this.statisticsService = statisticsService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<int> InitiateFight(int opponentId)
        {
            Fight fight = new Fight();

            Hero attacker = await this.heroService.GetCurrentHero();

            if (attacker.WorkStatus != WorkStatus.Idle)
            {
                throw new FarmHeroesException(
                    "You cannot attack while working.",
                    "You have to cancel or finish your work before trying to attack.",
                    "/Battlefield");
            }
            else if (attacker.Chronometer.CannotAttackHeroUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    "You cannot attack right now.",
                    "You have to wait until you can attack again. Take a rest, do something useful.",
                    "/Battlefield");
            }
            else if (attacker.Id == opponentId)
            {
                throw new FarmHeroesException(
                    "You cannot attack yourself.",
                    "Who would attack themselves? Go find somebody else!.",
                    "/Battlefield");
            }

            Hero defender = await this.heroService.GetHeroById(opponentId);

            if (defender.Fraction == attacker.Fraction)
            {
                throw new FarmHeroesException(
                    "You cannot attack a hero from your fraction.",
                    "Don't be a bad player. You can attack only players from the enemy fraction.",
                    "/Battlefield");
            }
            else if (attacker.Level.CurrentLevel + 3 < defender.Level.CurrentLevel
                    && attacker.Level.CurrentLevel - 3 > defender.Level.CurrentLevel)
            {
                throw new FarmHeroesException(
                    "The hero you attempted to attack is outside of your level range.",
                    "You can attack players with 3 levels below or above you, inclusively.",
                    "/Battlefield");
            }
            else if (defender.Chronometer.CannotBeAttackedUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    "You cannot attack a hero that still has defence.",
                    "This hero still has defence. After being attacked, one is guaranteed an hour of immunity.",
                    "/Battlefield");
            }

            int?[] attackerHits = new int?[5];
            int?[] defenderHits = new int?[5];
            string winnerName = string.Empty;
            int goldStolen = 0;

            for (int i = 0; i < Rounds; i++)
            {
                int attackerDamage = FightFormulas.CalculateHitDamage(
                    attacker.Characteristics.Attack,
                    defender.Characteristics.Defense,
                    attacker.Characteristics.Mastery,
                    defender.Characteristics.Mastery);

                await this.healthService.ReduceHealthById(defender.HealthId, attackerDamage);
                attackerHits[i] = attackerDamage;

                if (await this.healthService.CheckIfDead(defender.HealthId))
                {
                    winnerName = attacker.Name;
                    break;
                }

                int defenderDamage = FightFormulas.CalculateHitDamage(
                    defender.Characteristics.Attack,
                    attacker.Characteristics.Defense,
                    defender.Characteristics.Mastery,
                    attacker.Characteristics.Mastery);

                await this.healthService.ReduceHealthById(attacker.HealthId, defenderDamage);
                defenderHits[i] = defenderDamage;

                if (await this.healthService.CheckIfDead(attacker.HealthId))
                {
                    winnerName = defender.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(winnerName))
            {
                winnerName = attackerHits.Sum() > defenderHits.Sum() ? attacker.Name : defender.Name;
            }

            if (winnerName == attacker.Name)
            {
                goldStolen = ResourceFormulas.CalculateStolenGold(defender.ResourcePouch.Gold);

                await this.resourcePouchService.IncreaseGold(attacker.ResourcePouchId, goldStolen);
                await this.resourcePouchService.DecreaseGold(defender.ResourcePouchId, goldStolen);
                await this.levelService.GiveHeroExperienceById(attacker.LevelId, ExperiencePerWin);

                attacker.Statistics.TotalGoldStolen += goldStolen;
                attacker.Statistics.Wins++;
                defender.Statistics.Losses++;

                if (attacker.Statistics.MaximalGoldSteal < goldStolen)
                {
                    attacker.Statistics.MaximalGoldSteal = goldStolen;
                }
            }
            else if (winnerName == defender.Name)
            {
                goldStolen = ResourceFormulas.CalculateStolenGold(attacker.ResourcePouch.Gold);

                await this.resourcePouchService.IncreaseGold(defender.ResourcePouchId, goldStolen);
                await this.resourcePouchService.DecreaseGold(attacker.ResourcePouchId, goldStolen);
                await this.levelService.GiveHeroExperienceById(defender.LevelId, ExperiencePerWin);

                defender.Statistics.TotalGoldStolen += goldStolen;
                defender.Statistics.Wins++;
                attacker.Statistics.Losses++;

                if (defender.Statistics.MaximalGoldSteal < goldStolen)
                {
                    defender.Statistics.MaximalGoldSteal = goldStolen;
                }
            }

            attacker.Statistics.TotalFights++;
            defender.Statistics.TotalFights++;

            await this.statisticsService.UpdateStatistics(attacker.Statistics);
            await this.statisticsService.UpdateStatistics(defender.Statistics);

            await this.chronometerService.SetCannotAttackHeroUntilById(attacker.ChronometerId, MinutesUntilNextHeroAttack);
            await this.chronometerService.SetCannotBeAttackedUntilById(defender.ChronometerId, MinutesDefenseGranted);

            fight.WinnerName = winnerName;
            fight.GoldStolen = goldStolen;
            fight.ExperienceGained = ExperiencePerWin;
            fight.AttackerDamageDealt = (int)attackerHits.Sum();
            fight.DefenderDamageDealt = (int)defenderHits.Sum();
            fight.AttackerHealthLeft = attacker.Health.Current;
            fight.DefenderHealthLeft = defender.Health.Current;

            fight.AttackerId = attacker.Id;
            fight.AttackerName = attacker.Name;
            fight.DefenderId = defender.Id;
            fight.DefenderName = defender.Name;

            fight.AttackerAttack = attacker.Characteristics.Attack;
            fight.AttackerDefense = attacker.Characteristics.Defense;
            fight.AttackerMastery = attacker.Characteristics.Mastery;
            fight.AttackerMass = attacker.Characteristics.Mass;
            fight.DefenderAttack = defender.Characteristics.Attack;
            fight.DefenderDefense = defender.Characteristics.Defense;
            fight.DefenderMastery = defender.Characteristics.Mastery;
            fight.DefenderMass = defender.Characteristics.Mass;

            fight.AttackerHitOne = attackerHits[0];
            fight.AttackerHitTwo = attackerHits[1];
            fight.AttackerHitThree = attackerHits[2];
            fight.AttackerHitFour = attackerHits[3];
            fight.AttackerHitFive = attackerHits[4];
            fight.DefenderHitOne = defenderHits[0];
            fight.DefenderHitTwo = defenderHits[1];
            fight.DefenderHitThree = defenderHits[2];
            fight.DefenderHitFour = defenderHits[3];
            fight.DefenderHitFive = defenderHits[4];

            Fight fightEntity = this.context.Fights.AddAsync(fight).Result.Entity;

            attacker.HeroFights.Add(new HeroFight { Fight = fightEntity, Hero = attacker });
            defender.HeroFights.Add(new HeroFight { Fight = fightEntity, Hero = defender });

            await this.context.SaveChangesAsync();

            return fightEntity.Id;
        }

        public async Task<Fight> GetFightById(int id)
        {
            Fight fight = await this.context.Fights.FindAsync(id);

            return fight;
        }

        public async Task<TViewModel> GetFightViewModelById<TViewModel>(int id)
        {
            Fight fight = await this.GetFightById(id);
            TViewModel viewModel = this.mapper.Map<TViewModel>(fight);

            return viewModel;
        }
    }
}
