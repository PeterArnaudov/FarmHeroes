namespace FarmHeroes.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MappingModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Services.Models.Monsters;

    public class FightService : IFightService
    {
        private const int Rounds = 5;
        private const int ExperiencePerWin = 4;
        private const int MinutesUntilNextHeroAttack = 15;
        private const int MinutesUntilNextMonsterAttack = 15;
        private const int MinutesDefenseGranted = 60;
        private const int RandomMonsterGoldCost = 500;
        private const int MonsterCrystalCost = 1;
        private const string MonsterFightNotificationImageUrl = "https://i.ibb.co/my7rwD4/monster-fight-notifications.png";
        private const string HeroFightNotificationImageUrl = "https://i.ibb.co/T12Xb6j/player-battle-notifications.png";

        private readonly IHeroService heroService;
        private readonly IHealthService healthService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IChronometerService chronometerService;
        private readonly ILevelService levelService;
        private readonly IStatisticsService statisticsService;
        private readonly IMonsterService monsterService;
        private readonly INotificationService notificationService;
        private readonly IEquipmentService equipmentService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public FightService(IHeroService heroService, IHealthService healthService, IResourcePouchService resourcePouchService, IChronometerService chronometerService, ILevelService levelService, IStatisticsService statisticsService, IMonsterService monsterService, INotificationService notificationService, IEquipmentService equipmentService, FarmHeroesDbContext context, IMapper mapper)
        {
            this.heroService = heroService;
            this.healthService = healthService;
            this.resourcePouchService = resourcePouchService;
            this.chronometerService = chronometerService;
            this.levelService = levelService;
            this.statisticsService = statisticsService;
            this.monsterService = monsterService;
            this.notificationService = notificationService;
            this.equipmentService = equipmentService;
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

            EquippedSet attackerSet = await this.equipmentService.GetCurrentHeroEquipedSet();
            EquippedSet defenderSet = await this.equipmentService.GetEquippedSetById(defender.EquippedSetId);

            int attackerAttack = attacker.Characteristics.Attack + FightFormulas.CalculateAttackFromSet(attackerSet);
            int attackerDefense = attacker.Characteristics.Defense + FightFormulas.CalculateDefenseFromSet(attackerSet);
            int attackerMastery = attacker.Characteristics.Mastery + FightFormulas.CalculateMasteryFromSet(attackerSet);
            int attackerMass = attacker.Characteristics.Mass;
            int defenderAttack = defender.Characteristics.Attack + FightFormulas.CalculateAttackFromSet(defenderSet);
            int defenderDefense = defender.Characteristics.Defense + FightFormulas.CalculateDefenseFromSet(defenderSet);
            int defenderMastery = defender.Characteristics.Mastery + FightFormulas.CalculateMasteryFromSet(defenderSet);
            int defenderMass = defender.Characteristics.Mass;
            int?[] attackerHits = new int?[5];
            int?[] defenderHits = new int?[5];
            string winnerName = string.Empty;
            int goldStolen = 0;

            for (int i = 0; i < Rounds; i++)
            {
                int attackerDamage = FightFormulas.CalculateHitDamage(
                    attackerAttack,
                    defenderDefense,
                    attackerMastery,
                    defenderMastery);

                await this.healthService.ReduceHealthById(defender.HealthId, attackerDamage);
                attackerHits[i] = attackerDamage;

                if (await this.healthService.CheckIfDead(defender.HealthId))
                {
                    winnerName = attacker.Name;
                    break;
                }

                int defenderDamage = FightFormulas.CalculateHitDamage(
                    defenderAttack,
                    attackerDefense,
                    defenderMastery,
                    attackerMastery);

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
            fight.AttackerLevel = attacker.Level.CurrentLevel;
            fight.DefenderLevel = defender.Level.CurrentLevel;
            fight.AttackerAvatarUrl = attacker.AvatarUrl;
            fight.DefenderAvatarUrl = defender.AvatarUrl;

            fight.AttackerAttack = attackerAttack;
            fight.AttackerDefense = attackerDefense;
            fight.AttackerMastery = attackerMastery;
            fight.AttackerMass = attackerMass;
            fight.DefenderAttack = defenderAttack;
            fight.DefenderDefense = defenderDefense;
            fight.DefenderMastery = defenderMastery;
            fight.DefenderMass = defenderMass;

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

            Notification attackerNotification = new Notification()
            {
                ImageUrl = HeroFightNotificationImageUrl,
                Title = "Fight log",
                Link = $"/Battlefield/FightLog/{fightEntity.Id}",
                Content = $"You attacked {defender.Name}.",
                Gold = winnerName == attacker.Name ? goldStolen : goldStolen * -1,
                Experience = winnerName == attacker.Name ? ExperiencePerWin : default(int?),
                Type = NotificationType.HeroFight,
                Hero = attacker,
            };
            Notification defenderNotification = new Notification()
            {
                ImageUrl = HeroFightNotificationImageUrl,
                Title = "Fight log",
                Link = $"/Battlefield/FightLog/{fightEntity.Id}",
                Content = $"{attacker.Name} attacked you.",
                Gold = winnerName == defender.Name ? goldStolen : goldStolen * -1,
                Experience = winnerName == defender.Name ? ExperiencePerWin : default(int?),
                Type = NotificationType.HeroFight,
                Hero = defender,
            };
            await this.notificationService.AddNotification(attackerNotification);
            await this.notificationService.AddNotification(defenderNotification);

            return fightEntity.Id;
        }

        public async Task<int> InitiateMonsterFight(int? monsterLevel)
        {
            Random random = new Random();
            Fight fight = new Fight();

            Hero attacker = await this.heroService.GetCurrentHero();

            if (attacker.WorkStatus != WorkStatus.Idle)
            {
                throw new FarmHeroesException(
                    "You cannot attack while working.",
                    "You have to cancel or finish your work before trying to attack.",
                    "/Battlefield");
            }
            else if (attacker.Chronometer.CannotAttackMonsterUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    "You cannot attack right now.",
                    "You have to wait until you can attack a monster again. Take a rest, do something useful.",
                    "/Battlefield");
            }

            if (monsterLevel == null)
            {
                monsterLevel = random.Next(1, 3);

                await this.resourcePouchService.DecreaseCurrentHeroGold(RandomMonsterGoldCost);
            }
            else
            {
                await this.resourcePouchService.DecreaseCurrentHeroCrystals(MonsterCrystalCost);
            }

            Monster databaseMonster = await this.monsterService.GetMonsterByLevel((int)monsterLevel);
            FightMonster monster = await this.monsterService.GenerateFightMonster(databaseMonster);

            EquippedSet attackerSet = await this.equipmentService.GetCurrentHeroEquipedSet();

            int attackerAttack = attacker.Characteristics.Attack + FightFormulas.CalculateAttackFromSet(attackerSet);
            int attackerDefense = attacker.Characteristics.Defense + FightFormulas.CalculateDefenseFromSet(attackerSet);
            int attackerMastery = attacker.Characteristics.Mastery + FightFormulas.CalculateMasteryFromSet(attackerSet);
            int attackerMass = attacker.Characteristics.Mass;

            int?[] attackerHits = new int?[5];
            int?[] defenderHits = new int?[5];
            string winnerName = string.Empty;
            int goldStolen = 0;

            for (int i = 0; i < Rounds; i++)
            {
                int attackerDamage = FightFormulas.CalculateHitDamage(
                    attackerAttack,
                    monster.Characteristics.Defense,
                    attackerMastery,
                    monster.Characteristics.Mastery);

                monster.Health -= attackerDamage;
                if (monster.Health < 1)
                {
                    monster.Health = 1;
                }

                attackerHits[i] = attackerDamage;

                if (monster.Health == 1)
                {
                    winnerName = attacker.Name;
                    break;
                }

                int defenderDamage = FightFormulas.CalculateHitDamage(
                    monster.Characteristics.Attack,
                    attackerDefense,
                    monster.Characteristics.Mastery,
                    attackerMastery);

                await this.healthService.ReduceHealthById(attacker.HealthId, defenderDamage);
                defenderHits[i] = defenderDamage;

                if (await this.healthService.CheckIfDead(attacker.HealthId))
                {
                    winnerName = monster.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(winnerName))
            {
                winnerName = attackerHits.Sum() > defenderHits.Sum() ? attacker.Name : monster.Name;
            }

            if (winnerName == attacker.Name)
            {
                goldStolen = MonsterFormulas.CalculateReward(monster.Level, attacker.Level.CurrentLevel);

                await this.resourcePouchService.IncreaseGold(attacker.ResourcePouchId, goldStolen);
                await this.levelService.GiveHeroExperienceById(attacker.LevelId, monster.Level);

                attacker.Statistics.EarnedFromMonsters += goldStolen;
                attacker.Statistics.MonstersDefeated++;
            }
            else if (winnerName == monster.Name)
            {
                goldStolen = ResourceFormulas.CalculateStolenGold(attacker.ResourcePouch.Gold);

                await this.resourcePouchService.DecreaseGold(attacker.ResourcePouchId, goldStolen);
            }

            await this.statisticsService.UpdateStatistics(attacker.Statistics);

            await this.chronometerService.SetCannotAttackMonsterUntilById(attacker.ChronometerId, MinutesUntilNextMonsterAttack);

            fight.WinnerName = winnerName;
            fight.GoldStolen = goldStolen;
            fight.ExperienceGained = monster.Level;
            fight.AttackerDamageDealt = (int)attackerHits.Sum();
            fight.DefenderDamageDealt = (int)defenderHits.Sum();
            fight.AttackerHealthLeft = attacker.Health.Current;
            fight.DefenderHealthLeft = monster.Health;

            fight.AttackerId = attacker.Id;
            fight.AttackerName = attacker.Name;
            fight.DefenderName = monster.Name;
            fight.AttackerLevel = attacker.Level.CurrentLevel;
            fight.DefenderLevel = monster.Level;
            fight.AttackerAvatarUrl = attacker.AvatarUrl;
            fight.DefenderAvatarUrl = monster.AvatarUrl;

            fight.AttackerAttack = attackerAttack;
            fight.AttackerDefense = attackerDefense;
            fight.AttackerMastery = attackerMastery;
            fight.AttackerMass = attackerMass;
            fight.DefenderAttack = monster.Characteristics.Attack;
            fight.DefenderDefense = monster.Characteristics.Defense;
            fight.DefenderMastery = monster.Characteristics.Mastery;
            fight.DefenderMass = monster.Characteristics.Mass;

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
            await this.context.SaveChangesAsync();

            Notification notification = new Notification()
            {
                ImageUrl = MonsterFightNotificationImageUrl,
                Title = "Fight log",
                Link = $"/Battlefield/FightLog/{fightEntity.Id}",
                Content = $"You attacked {monster.Name}.",
                Gold = winnerName == attacker.Name ? goldStolen : goldStolen * -1,
                Experience = winnerName == attacker.Name ? monster.Level : default(int?),
                Type = NotificationType.MonsterFight,
                Hero = attacker,
            };
            await this.notificationService.AddNotification(notification);

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
