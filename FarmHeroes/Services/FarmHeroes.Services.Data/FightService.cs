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
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Services.Models.Monsters;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Constants;

    public class FightService : IFightService
    {
        private const int Rounds = 5;
        private const int ExperiencePerWin = 4;
        private const int SecondsUntilNextHeroAttack = 600;
        private const int SecondsUntilNextMonsterAttack = 300;
        private const int SecondsDefenseGranted = 1800;
        private const int RandomMonsterGoldCost = 50;
        private const int MonsterCrystalCost = 1;
        private const int RandomMonsterMinimumLevel = 1;
        private const int RandomMonsterMaximumLevel = 3;
        private const int LevelBoundary = 3;
        private const string MonsterFightNotificationImageUrl = "/images/notifications/monster-fight-notification.png";
        private const string HeroFightNotificationImageUrl = "/images/notifications/hero-battle-notification.png";
        private const string FightNotificationTitle = "Fight log";
        private const string FightNotificationLink = "/Battlefield/FightLog/{0}";
        private const string FightNotificationAttacker = "You attacked {0}.";
        private const string FightNotificationDefender = "{0} attacked you.";

        private readonly IHeroService heroService;
        private readonly IHealthService healthService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly IChronometerService chronometerService;
        private readonly ILevelService levelService;
        private readonly IStatisticsService statisticsService;
        private readonly IMonsterService monsterService;
        private readonly INotificationService notificationService;
        private readonly IEquipmentService equipmentService;
        private readonly IAmuletBagService amuletBagService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public FightService(IHeroService heroService, IHealthService healthService, IResourcePouchService resourcePouchService, IChronometerService chronometerService, ILevelService levelService, IStatisticsService statisticsService, IMonsterService monsterService, INotificationService notificationService, IEquipmentService equipmentService, IAmuletBagService amuletBagService, FarmHeroesDbContext context, IMapper mapper)
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
            this.amuletBagService = amuletBagService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<int> InitiateFight(int opponentId)
        {
            Fight fight = new Fight();

            Hero attacker = await this.heroService.GetHero();

            this.CheckIfHeroIsWorking(attacker);
            this.CheckIfHeroCanAttackPlayer(attacker);
            this.CheckIfHeroAttackThemselves(attacker, opponentId);

            await this.amuletBagService.EquipAmulet("PlayerAttack");

            Hero defender = await this.heroService.GetHero(opponentId);

            this.CheckIfOpponentsAreSameFraction(attacker, defender);
            this.CheckIfInLevelBoundary(attacker, defender);
            this.CheckIfHeroCanBeAttacked(defender);

            EquippedSet attackerSet = await this.equipmentService.GetEquippedSet();
            EquippedSet defenderSet = await this.equipmentService.GetEquippedSet(defender.EquippedSetId);

            int attackerAttack = FightFormulas.CalculateAttack(attacker);
            int attackerDefense = FightFormulas.CalculateDefense(attacker);
            int attackerMastery = FightFormulas.CalculateMastery(attacker);
            int attackerDexterity = FightFormulas.CalculateDexterity(attacker);
            int attackerMass = attacker.Characteristics.Mass;
            int defenderAttack = FightFormulas.CalculateAttack(defender);
            int defenderDefense = FightFormulas.CalculateDefense(defender);
            int defenderMastery = FightFormulas.CalculateMastery(defender);
            int defenderDexterity = FightFormulas.CalculateDexterity(defender);
            int defenderMass = defender.Characteristics.Mass;
            HitType[] attackerHitTypes = new HitType[Rounds];
            HitType[] defenderHitTypes = new HitType[Rounds];
            int?[] attackerHits = new int?[Rounds];
            int?[] defenderHits = new int?[Rounds];
            string winnerName = string.Empty;
            int goldStolen = 0;

            for (int i = 0; i < Rounds; i++)
            {
                bool isCrit = FightFormulas.IsCrit(
                    attackerMastery,
                    defenderDexterity,
                    attackerSet.Amulet?.Name == AmuletNames.Criticum ? attackerSet.Amulet.Bonus : 0);

                bool isDodged = FightFormulas.IsDodged(attackerAttack, defenderDexterity, 0);

                int attackerDamage = FightFormulas.CalculateHitDamage(
                    attackerAttack,
                    defenderDefense,
                    isCrit,
                    isDodged,
                    attackerSet.Amulet?.Name == AmuletNames.Fatty ? attackerSet.Amulet.Bonus : 0);

                await this.healthService.ReduceHealth(attackerDamage, defender.HealthId);
                attackerHitTypes[i] = isDodged ? HitType.Dodged : isCrit ? HitType.Critical : HitType.Normal;
                attackerHits[i] = attackerDamage;

                if (await this.healthService.CheckIfDead(defender.HealthId))
                {
                    winnerName = attacker.Name;
                    break;
                }

                isCrit = FightFormulas.IsCrit(
                    defenderMastery,
                    attackerDexterity,
                    defenderSet.Amulet?.Name == AmuletNames.Criticum ? defenderSet.Amulet.Bonus : 0);

                isDodged = FightFormulas.IsDodged(defenderAttack, attackerDexterity, 0);

                int defenderDamage = FightFormulas.CalculateHitDamage(
                    defenderAttack,
                    attackerDefense,
                    isCrit,
                    isDodged,
                    defenderSet.Amulet?.Name == AmuletNames.Fatty ? defenderSet.Amulet.Bonus : 0);

                await this.healthService.ReduceHealth(defenderDamage, attacker.HealthId);
                defenderHitTypes[i] = isDodged ? HitType.Dodged : isCrit ? HitType.Critical : HitType.Normal;
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
                goldStolen = ResourceFormulas.CalculateStolenGold(
                    defender.ResourcePouch.Gold,
                    this.GetGoldSafeBonus(defender));

                await this.resourcePouchService.IncreaseResource(ResourceNames.Gold, goldStolen, attacker.ResourcePouchId);
                await this.resourcePouchService.DecreaseResource(ResourceNames.Gold, goldStolen, defender.ResourcePouchId);
                await this.levelService.GiveHeroExperience(ExperiencePerWin, attacker.LevelId);

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
                goldStolen = ResourceFormulas.CalculateStolenGold(
                    attacker.ResourcePouch.Gold,
                    this.GetGoldSafeBonus(attacker));

                await this.resourcePouchService.IncreaseResource(ResourceNames.Gold, goldStolen, defender.ResourcePouchId);
                await this.resourcePouchService.DecreaseResource(ResourceNames.Gold, goldStolen, attacker.ResourcePouchId);
                await this.levelService.GiveHeroExperience(ExperiencePerWin, defender.LevelId);

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

            await this.chronometerService.SetCannotAttackHeroUntil(SecondsUntilNextHeroAttack, attacker.ChronometerId);
            await this.chronometerService.SetCannotBeAttackedUntil(SecondsDefenseGranted, defender.ChronometerId);

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
            fight.AttackerDexterity = attackerDexterity;
            fight.DefenderAttack = defenderAttack;
            fight.DefenderDefense = defenderDefense;
            fight.DefenderMastery = defenderMastery;
            fight.DefenderMass = defenderMass;
            fight.DefenderDexterity = defenderDexterity;

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

            fight.AttackerHitOneType = attackerHitTypes[0];
            fight.AttackerHitTwoType = attackerHitTypes[1];
            fight.AttackerHitThreeType = attackerHitTypes[2];
            fight.AttackerHitFourType = attackerHitTypes[3];
            fight.AttackerHitFiveType = attackerHitTypes[4];
            fight.DefenderHitOneType = defenderHitTypes[0];
            fight.DefenderHitTwoType = defenderHitTypes[1];
            fight.DefenderHitThreeType = defenderHitTypes[2];
            fight.DefenderHitFourType = defenderHitTypes[3];
            fight.DefenderHitFiveType = defenderHitTypes[4];

            Fight fightEntity = this.context.Fights.AddAsync(fight).Result.Entity;

            attacker.HeroFights.Add(new HeroFight { Fight = fightEntity, Hero = attacker });
            defender.HeroFights.Add(new HeroFight { Fight = fightEntity, Hero = defender });

            await this.context.SaveChangesAsync();

            Notification attackerNotification = new Notification()
            {
                ImageUrl = HeroFightNotificationImageUrl,
                Title = FightNotificationTitle,
                Link = string.Format(FightNotificationLink, fightEntity.Id),
                Content = string.Format(FightNotificationAttacker, defender.Name),
                Gold = winnerName == attacker.Name ? goldStolen : goldStolen * -1,
                Experience = winnerName == attacker.Name ? ExperiencePerWin : default(int?),
                Type = NotificationType.HeroFight,
                Hero = attacker,
            };
            Notification defenderNotification = new Notification()
            {
                ImageUrl = HeroFightNotificationImageUrl,
                Title = FightNotificationTitle,
                Link = string.Format(FightNotificationLink, fightEntity.Id),
                Content = string.Format(FightNotificationDefender, attacker.Name),
                Gold = winnerName == defender.Name ? goldStolen : goldStolen * -1,
                Experience = winnerName == defender.Name ? ExperiencePerWin : default(int?),
                Type = NotificationType.HeroFight,
                Hero = defender,
            };
            await this.notificationService.AddNotification(attackerNotification);
            await this.notificationService.AddNotification(defenderNotification);

            await this.amuletBagService.EquipAmulet("Idle");

            return fightEntity.Id;
        }

        public async Task<int> InitiateMonsterFight(int monsterLevel = 0)
        {
            Random random = new Random();
            Fight fight = new Fight();

            Hero attacker = await this.heroService.GetHero();

            this.CheckIfHeroIsWorking(attacker);
            this.CheckIfHeroCanAttackMonster(attacker);

            await this.amuletBagService.EquipAmulet("MonsterAttack");

            if (monsterLevel == 0)
            {
                monsterLevel = random.Next(RandomMonsterMinimumLevel, RandomMonsterMaximumLevel);

                await this.resourcePouchService.DecreaseResource(ResourceNames.Gold, RandomMonsterGoldCost);
            }
            else
            {
                await this.resourcePouchService.DecreaseResource(ResourceNames.Crystals, MonsterCrystalCost);
            }

            Monster databaseMonster = await this.monsterService.GetMonsterByLevel(monsterLevel);
            FightMonster monster = await this.monsterService.GenerateFightMonster(databaseMonster);

            EquippedSet attackerSet = await this.equipmentService.GetEquippedSet();

            int attackerAttack = FightFormulas.CalculateAttack(attacker);
            int attackerDefense = FightFormulas.CalculateDefense(attacker);
            int attackerMastery = FightFormulas.CalculateMastery(attacker);
            int attackerDexterity = FightFormulas.CalculateDexterity(attacker);
            int attackerMass = attacker.Characteristics.Mass;

            HeroAmulet heroAmulet = attacker.EquippedSet.Amulet;

            if (heroAmulet?.Name == AmuletNames.Undergrounder)
            {
                attackerAttack = (int)(attackerAttack * (1 + (heroAmulet.Bonus / 100)));
                attackerDefense = (int)(attackerDefense * (1 + (heroAmulet.Bonus / 100)));
                attackerMastery = (int)(attackerMastery * (1 + (heroAmulet.Bonus / 100)));
                attackerDexterity = (int)(attackerDexterity * (1 + (heroAmulet.Bonus / 100)));
            }

            int?[] attackerHits = new int?[Rounds];
            int?[] defenderHits = new int?[Rounds];
            HitType[] attackerHitTypes = new HitType[Rounds];
            HitType[] defenderHitTypes = new HitType[Rounds];
            string winnerName = string.Empty;
            int goldStolen = 0;

            for (int i = 0; i < Rounds; i++)
            {
                bool isCrit = FightFormulas.IsCrit(
                    attackerMastery,
                    monster.Characteristics.Dexterity,
                    attackerSet.Amulet?.Name == AmuletNames.Criticum ? attackerSet.Amulet.Bonus : 0);

                bool isDodged = FightFormulas.IsDodged(attackerAttack, monster.Characteristics.Dexterity, 0);

                int attackerDamage = FightFormulas.CalculateHitDamage(
                    attackerAttack,
                    monster.Characteristics.Defense,
                    isCrit,
                    isDodged,
                    attackerSet.Amulet?.Name == AmuletNames.Fatty ? attackerSet.Amulet.Bonus : 0);

                monster.Health -= attackerDamage;
                if (monster.Health < 1)
                {
                    monster.Health = 1;
                }

                attackerHits[i] = attackerDamage;
                attackerHitTypes[i] = isDodged ? HitType.Dodged : isCrit ? HitType.Critical : HitType.Normal;

                if (monster.Health == 1)
                {
                    winnerName = attacker.Name;
                    break;
                }

                isCrit = FightFormulas.IsCrit(monster.Characteristics.Attack, attackerDexterity, 0);
                isDodged = FightFormulas.IsDodged(monster.Characteristics.Attack, attackerDexterity, 0);

                int defenderDamage = FightFormulas.CalculateHitDamage(
                    monster.Characteristics.Attack,
                    attackerDefense,
                    isCrit,
                    isDodged,
                    0);

                await this.healthService.ReduceHealth(defenderDamage, attacker.HealthId);
                defenderHits[i] = defenderDamage;
                defenderHitTypes[i] = isDodged ? HitType.Dodged : isCrit ? HitType.Critical : HitType.Normal;

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
                goldStolen = MonsterFormulas.CalculateReward(databaseMonster, attacker.Level.CurrentLevel);

                await this.resourcePouchService.IncreaseResource(ResourceNames.Gold, goldStolen, attacker.ResourcePouchId);
                await this.levelService.GiveHeroExperience(monster.Level, attacker.LevelId);

                attacker.Statistics.EarnedFromMonsters += goldStolen;
                attacker.Statistics.MonstersDefeated++;
            }
            else if (winnerName == monster.Name)
            {
                goldStolen = ResourceFormulas.CalculateStolenGold(
                    attacker.ResourcePouch.Gold,
                    this.GetGoldSafeBonus(attacker));

                await this.resourcePouchService.DecreaseResource(ResourceNames.Gold, goldStolen, attacker.ResourcePouchId);
            }

            await this.statisticsService.UpdateStatistics(attacker.Statistics);

            await this.chronometerService.SetCannotAttackMonsterUntil(SecondsUntilNextMonsterAttack, attacker.ChronometerId);

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
            fight.AttackerDexterity = attackerDexterity;
            fight.DefenderAttack = monster.Characteristics.Attack;
            fight.DefenderDefense = monster.Characteristics.Defense;
            fight.DefenderMastery = monster.Characteristics.Mastery;
            fight.DefenderMass = monster.Characteristics.Mass;
            fight.DefenderDexterity = monster.Characteristics.Dexterity;

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

            fight.AttackerHitOneType = attackerHitTypes[0];
            fight.AttackerHitTwoType = attackerHitTypes[1];
            fight.AttackerHitThreeType = attackerHitTypes[2];
            fight.AttackerHitFourType = attackerHitTypes[3];
            fight.AttackerHitFiveType = attackerHitTypes[4];
            fight.DefenderHitOneType = defenderHitTypes[0];
            fight.DefenderHitTwoType = defenderHitTypes[1];
            fight.DefenderHitThreeType = defenderHitTypes[2];
            fight.DefenderHitFourType = defenderHitTypes[3];
            fight.DefenderHitFiveType = defenderHitTypes[4];

            Fight fightEntity = this.context.Fights.AddAsync(fight).Result.Entity;
            attacker.HeroFights.Add(new HeroFight { Fight = fightEntity, Hero = attacker });
            await this.context.SaveChangesAsync();

            Notification notification = new Notification()
            {
                ImageUrl = MonsterFightNotificationImageUrl,
                Title = FightNotificationTitle,
                Link = string.Format(FightNotificationLink, fightEntity.Id),
                Content = string.Format(FightNotificationAttacker, monster.Name),
                Gold = winnerName == attacker.Name ? goldStolen : goldStolen * -1,
                Experience = winnerName == attacker.Name ? monster.Level : default(int?),
                Type = NotificationType.MonsterFight,
                Hero = attacker,
            };
            await this.notificationService.AddNotification(notification);

            await this.amuletBagService.EquipAmulet("Idle");

            return fightEntity.Id;
        }

        public async Task<Fight> GetFight(int id)
        {
            Fight fight = await this.context.Fights.FindAsync(id);

            return fight;
        }

        public async Task<TViewModel> GetFightViewModel<TViewModel>(int id)
        {
            Fight fight = await this.GetFight(id);
            TViewModel viewModel = this.mapper.Map<TViewModel>(fight);

            return viewModel;
        }

        private void CheckIfInLevelBoundary(Hero attacker, Hero defender)
        {
            if (attacker.Level.CurrentLevel + LevelBoundary < defender.Level.CurrentLevel
                 && attacker.Level.CurrentLevel - LevelBoundary > defender.Level.CurrentLevel)
            {
                throw new FarmHeroesException(
                    FightExceptionMessages.CannotAttackExceptionMessage,
                    FightExceptionMessages.CannotAttackOutsideOfLevelRangeInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfOpponentsAreSameFraction(Hero attacker, Hero defender)
        {
            if (defender.Fraction == attacker.Fraction)
            {
                throw new FarmHeroesException(
                    FightExceptionMessages.CannotAttackExceptionMessage,
                    FightExceptionMessages.CannotAttackSameFractionInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfHeroAttackThemselves(Hero hero, int opponentId)
        {
            if (hero.Id == opponentId)
            {
                throw new FarmHeroesException(
                    FightExceptionMessages.CannotAttackExceptionMessage,
                    FightExceptionMessages.CannotAttackThemselvesInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfHeroIsWorking(Hero hero)
        {
            if (hero.WorkStatus != WorkStatus.Idle && hero.WorkStatus != WorkStatus.Dungeon)
            {
                throw new FarmHeroesException(
                    FightExceptionMessages.CannotAttackExceptionMessage,
                    FightExceptionMessages.CannotAttackWhileWorkingInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfHeroCanAttackPlayer(Hero hero)
        {
            if (hero.Chronometer.CannotAttackHeroUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    FightExceptionMessages.CannotAttackExceptionMessage,
                    FightExceptionMessages.CannotAttackWhileRestingInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfHeroCanBeAttacked(Hero hero)
        {
            if (hero.Chronometer.CannotBeAttackedUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    FightExceptionMessages.CannotAttackExceptionMessage,
                    FightExceptionMessages.CannotAttackHeroesWithImmunity,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfHeroCanAttackMonster(Hero hero)
        {
            if (hero.Chronometer.CannotAttackMonsterUntil > DateTime.UtcNow && hero.WorkStatus != WorkStatus.Dungeon)
            {
                throw new FarmHeroesException(
                    FightExceptionMessages.CannotAttackExceptionMessage,
                    FightExceptionMessages.CannotAttackWhileRestingInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }

        private double GetGoldSafeBonus(Hero hero)
        {
            HeroBonus safe = hero.Inventory.Bonuses.SingleOrDefault(b => b.Name == BonusNames.GoldSafe);

            return safe != null && safe.ActiveUntil > DateTime.UtcNow ? safe.Bonus : 0;
        }
    }
}
