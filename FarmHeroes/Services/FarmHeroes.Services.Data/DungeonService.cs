namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using System;
    using System.Threading.Tasks;

    public class DungeonService : IDungeonService
    {
        private const int KeyPrice = 10;
        private const int WalkingDurationInSeconds = 120;
        private const int TimeBetweenDungeonsInSeconds = 3600;
        private const int MonstersPerFloor = 3;
        private const string MonsterFightNotificationImageUrl = "/images/notifications/monster-fight-notification.png";
        private const string DungeonNotificationTitle = "Dungeon report";
        private const string DungeonExitNotificationContent = "You exited the dungeon voluntarily. You reached {0} floor.";
        private const string DungeonDefeatNotificationContent = "You were defeated by a monster on {0} floor.";

        private readonly IResourcePouchService resourcePouchService;
        private readonly IHeroService heroService;
        private readonly IChronometerService chronometerService;
        private readonly IFightService fightService;
        private readonly INotificationService notificationService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public DungeonService(IResourcePouchService resourcePouchService, IHeroService heroService, IChronometerService chronometerService, IFightService fightService, INotificationService notificationService, FarmHeroesDbContext context, IMapper mapper)
        {
            this.resourcePouchService = resourcePouchService;
            this.heroService = heroService;
            this.chronometerService = chronometerService;
            this.fightService = fightService;
            this.notificationService = notificationService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task BuyKey()
        {
            await this.resourcePouchService.DecreaseResource(ResourceNames.Crystals, KeyPrice);
            await this.resourcePouchService.IncreaseResource(ResourceNames.DungeonKeys, 1);
        }

        public async Task StartDungeon()
        {
            Hero hero = await this.heroService.GetHero();

            this.CheckIfHeroCanStartDungeon(hero);
            this.CheckIfHeroAlreadyInDungeon(hero);

            await this.resourcePouchService.DecreaseResource(ResourceNames.DungeonKeys, 1);
            await this.chronometerService.SetWorkUntil(WalkingDurationInSeconds, WorkStatus.Dungeon);

            DungeonInformation dungeonInformation = await this.GetDungeonInformation();

            dungeonInformation.CurrentFloor = 1;
            dungeonInformation.MonstersDefeatedOnCurrentFloor = 0;

            await this.context.SaveChangesAsync();
        }

        public async Task AttackMonster()
        {
            DungeonInformation dungeonInformation = await this.GetDungeonInformation();

            int fightId = await this.fightService.InitiateMonsterFight(dungeonInformation.CurrentFloor);
            Fight fight = await this.context.Fights.FindAsync(fightId);
            Hero hero = await this.heroService.GetHero();

            if (fight.WinnerName != hero.Name)
            {
                await this.EndDungeon(false);
            }
            else
            {
                hero.WorkStatus = WorkStatus.DungeonIdle;
                dungeonInformation.MonstersDefeatedOnCurrentFloor++;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task WalkOnFloor()
        {
            Hero hero = await this.heroService.GetHero();

            this.CheckIfHeroIsInDungeon(hero);

            DungeonInformation dungeonInformation = await this.GetDungeonInformation();

            this.CheckIfThereAreMonstersOnFloor(dungeonInformation.MonstersDefeatedOnCurrentFloor);

            await this.chronometerService.SetWorkUntil(WalkingDurationInSeconds, WorkStatus.Dungeon);
        }

        public async Task GoToNextFloor()
        {
            Hero hero = await this.heroService.GetHero();

            this.CheckIfHeroIsInDungeon(hero);

            await this.chronometerService.SetWorkUntil(WalkingDurationInSeconds, WorkStatus.Dungeon);

            DungeonInformation dungeonInformation = await this.GetDungeonInformation();
            dungeonInformation.CurrentFloor++;
            dungeonInformation.MonstersDefeatedOnCurrentFloor = 0;

            await this.context.SaveChangesAsync();
        }

        public async Task EndDungeon(bool voluntarily = true)
        {
            await this.chronometerService.NullifyWorkUntil();
            await this.chronometerService.SetCannotDungeonUntil(TimeBetweenDungeonsInSeconds);

            Notification notification = new Notification()
            {
                ImageUrl = MonsterFightNotificationImageUrl,
                Title = DungeonNotificationTitle,
                Content = voluntarily
                    ? string.Format(DungeonExitNotificationContent, this.GetDungeonInformation().Result.CurrentFloor)
                    : string.Format(DungeonDefeatNotificationContent, this.GetDungeonInformation().Result.CurrentFloor),
                Type = NotificationType.Dungeon,
                Hero = await this.heroService.GetHero(),
            };
            await this.notificationService.AddNotification(notification);
        }

        public async Task<T> GetDungeonViewModel<T>()
        {
            Hero hero = await this.heroService.GetHero();

            return this.mapper.Map<T>(hero);
        }

        private async Task<DungeonInformation> GetDungeonInformation()
        {
            Hero hero = await this.heroService.GetHero();

            return hero.DungeonInformation;
        }

        private void CheckIfThereAreMonstersOnFloor(int monstersDefeated)
        {
            if (monstersDefeated == MonstersPerFloor)
            {
                throw new FarmHeroesException(
                    DungeonExceptionMessages.NoMonstersOnFloorMessage,
                    DungeonExceptionMessages.NoMonstersOnFloorInstruction,
                    Redirects.DungeonWalkingRedirect);
            }
        }

        private void CheckIfHeroCanStartDungeon(Hero hero)
        {
            if (hero.Chronometer.CannotDungeonUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    DungeonExceptionMessages.HeroCannotStartDungeonMessage,
                    DungeonExceptionMessages.HeroCannotStartDungeonInstruction,
                    Redirects.DungeonRedirect);
            }
        }

        private void CheckIfHeroAlreadyInDungeon(Hero hero)
        {
            if (hero.WorkStatus == WorkStatus.Dungeon || hero.WorkStatus == WorkStatus.DungeonIdle)
            {
                throw new FarmHeroesException(
                    DungeonExceptionMessages.HeroAlreadyInDungeonMessage,
                    DungeonExceptionMessages.HeroAlreadyInDungeonInstruction,
                    Redirects.DungeonWalkingRedirect);
            }
        }

        private void CheckIfHeroIsInDungeon(Hero hero)
        {
            if (hero.WorkStatus != WorkStatus.DungeonIdle)
            {
                throw new FarmHeroesException(
                    DungeonExceptionMessages.HeroNotInDungeonMessage,
                    DungeonExceptionMessages.HeroNotInDungeonInstruction,
                    Redirects.DungeonRedirect);
            }
        }
    }
}
