﻿namespace Farmheroes.Services.Data.Utilities
{
    using System;
    using System.Linq;
    using AutoMapper;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Data.Models.ShopModels;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.BattlefieldModels;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using FarmHeroes.Web.ViewModels.FarmModels;
    using FarmHeroes.Web.ViewModels.FightModels;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using FarmHeroes.Web.ViewModels.InventoryModels;
    using FarmHeroes.Web.ViewModels.MineModels;
    using FarmHeroes.Web.ViewModels.NotificationModels;
    using FarmHeroes.Web.ViewModels.ShopModels;
    using FarmHeroes.Web.ViewModels.SmithModels;
    using FarmHeroes.Web.ViewModels.StatisticsModels;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;

    public class FarmHeroesProfile : Profile
    {
        public FarmHeroesProfile()
        {
            this.CreateMap<HeroCreateInputModel, Hero>();

            this.CreateMap<Hero, HeroOverviewViewModel>()
                .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.Name))
                .ForMember(x => x.Level, cfg => cfg.MapFrom(x => x.Level.CurrentLevel))
                .ForMember(x => x.CurrentExperience, cfg => cfg.MapFrom(x => x.Level.CurrentExperience))
                .ForMember(x => x.NeededExperience, cfg => cfg.MapFrom(x => x.Level.NeededExperience))
                .ForMember(x => x.ExperiencePercent, cfg => cfg.MapFrom(x => Math.Round((double)(100 * x.Level.CurrentExperience / x.Level.NeededExperience))))
                .ForMember(x => x.Attack, cfg => cfg.MapFrom(x => x.Characteristics.Attack))
                .ForMember(x => x.Defense, cfg => cfg.MapFrom(x => x.Characteristics.Defense))
                .ForMember(x => x.Mass, cfg => cfg.MapFrom(x => x.Characteristics.Mass))
                .ForMember(x => x.Mastery, cfg => cfg.MapFrom(x => x.Characteristics.Mastery))
                .ForMember(x => x.Fights, cfg => cfg.MapFrom(x => x.Statistics.TotalFights))
                .ForMember(x => x.Wins, cfg => cfg.MapFrom(x => x.Statistics.Wins))
                .ForMember(x => x.EquippedSetHelmet, cfg => cfg.MapFrom(x => x.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Helmet)))
                .ForMember(x => x.EquippedSetArmor, cfg => cfg.MapFrom(x => x.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Armor)))
                .ForMember(x => x.EquippedSetWeapon, cfg => cfg.MapFrom(x => x.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Weapon)))
                .ForMember(x => x.EquippedSetShield, cfg => cfg.MapFrom(x => x.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Shield)));

            this.CreateMap<Characteristics, CharacteristicsPracticeViewModel>()
                .ForMember(x => x.AttackPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateAttackPrice(x.Attack)))
                .ForMember(x => x.DefensePrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateDefensePrice(x.Defense)))
                .ForMember(x => x.MassPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateMassPrice(x.Mass)))
                .ForMember(x => x.MasteryPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateMasteryPrice(x.Mastery)));

            this.CreateMap<Hero, MineViewModel>()
                .ForMember(x => x.WorkUntil, cfg => cfg.MapFrom(x => x.Chronometer.WorkUntil))
                .ForMember(x => x.DoesWork, cfg => cfg.MapFrom(x => x.WorkStatus == WorkStatus.Mine))
                .ForMember(x => x.IsWorkFinished, cfg => cfg.MapFrom(x => x.Chronometer.WorkUntil < DateTime.UtcNow && x.WorkStatus == WorkStatus.Mine));

            this.CreateMap<Hero, FarmViewModel>()
                .ForMember(x => x.WorkUntil, cfg => cfg.MapFrom(x => x.Chronometer.WorkUntil))
                .ForMember(x => x.DoesWork, cfg => cfg.MapFrom(x => x.WorkStatus == WorkStatus.Farm))
                .ForMember(x => x.IsWorkFinished, cfg => cfg.MapFrom(x => x.Chronometer.WorkUntil < DateTime.UtcNow && x.WorkStatus == WorkStatus.Farm))
                .ForMember(x => x.Salary, cfg => cfg.MapFrom(x => FarmFormulas.CalculateFarmSalaryPerHour(x.Level.CurrentLevel)));

            this.CreateMap<Hero, BattlefieldViewModel>()
                .ForMember(x => x.CanAttackHero, cfg => cfg.MapFrom(x => x.Chronometer.CannotAttackHeroUntil < DateTime.UtcNow || x.Chronometer.CannotAttackHeroUntil == null))
                .ForMember(x => x.CannotAttackHeroUntil, cfg => cfg.MapFrom(x => x.Chronometer.CannotAttackHeroUntil))
                .ForMember(x => x.CanAttackMonster, cfg => cfg.MapFrom(x => x.Chronometer.CannotAttackMonsterUntil < DateTime.UtcNow || x.Chronometer.CannotAttackMonsterUntil == null))
                .ForMember(x => x.IsPatrolFinished, cfg => cfg.MapFrom(x => x.Chronometer.WorkUntil < DateTime.UtcNow))
                .ForMember(x => x.CannotAttackMonsterUntil, cfg => cfg.MapFrom(x => x.Chronometer.CannotAttackMonsterUntil))
                .ForMember(x => x.CanGoOnPatrol, cfg => cfg.MapFrom(x => x.WorkStatus == WorkStatus.Idle))
                .ForMember(x => x.PatrolUntil, cfg => cfg.MapFrom(x => x.Chronometer.WorkUntil))
                .ForMember(x => x.IsOnPatrol, cfg => cfg.MapFrom(x => x.WorkStatus == WorkStatus.Battlefield));

            this.CreateMap<Health, HeroHealthViewComponentModel>()
                .ForMember(x => x.Percent, cfg => cfg.MapFrom(x => Math.Round((double)(100 * x.Current / x.Maximum))));

            this.CreateMap<Hero, BattlefieldOpponentViewModel>()
                .ForMember(x => x.Attack, cfg => cfg.MapFrom(x => x.Characteristics.Attack))
                .ForMember(x => x.Defense, cfg => cfg.MapFrom(x => x.Characteristics.Defense))
                .ForMember(x => x.Mastery, cfg => cfg.MapFrom(x => x.Characteristics.Mastery))
                .ForMember(x => x.Mass, cfg => cfg.MapFrom(x => x.Characteristics.Mass));

            this.CreateMap<Hero[], BattlefieldGetOpponentsViewModel>()
                .ForMember(x => x.Opponents, cfg => cfg.MapFrom(x => x));

            this.CreateMap<Inventory, InventoryViewModel>()
                .ForMember(x => x.UpgradeCost, cfg => cfg.MapFrom(x => InventoryFormulas.CalculateUpgradeCost(x.MaximumCapacity)));

            this.CreateMap<HeroEquipment, SmithEquipmentViewModel>()
                .ForMember(x => x.UpgradeCost, cfg => cfg.MapFrom(x => SmithFormulas.CalculateEquipmentUpgradeCost(x)));

            this.CreateMap<HeroAmulet, SmithAmuletViewModel>()
                .ForMember(x => x.UpgradeCost, cfg => cfg.MapFrom(x => SmithFormulas.CalculateAmuletUpgradeCost(x)));

            this.CreateMap<ShopEquipment[], ShopViewModel>()
                .ForMember(x => x.Items, cfg => cfg.MapFrom(x => x));

            this.CreateMap<ShopAmulet[], AmuletShopViewModel>()
                .ForMember(x => x.Items, cfg => cfg.MapFrom(x => x));

            this.CreateMap<Inventory, SmithViewModel>()
                .ForMember(x => x.Items, cfg => cfg.MapFrom(x => x.Items.Where(x => x.Level < 25)))
                .ForMember(x => x.Amulets, cfg => cfg.MapFrom(x => x.Amulets.Where(x => x.Level < 100)));

            this.CreateMap<ShopEquipment, EquipmentViewModel>();

            this.CreateMap<HeroEquipment, EquipmentViewModel>();

            this.CreateMap<ShopAmulet, AmuletViewModel>();

            this.CreateMap<HeroAmulet, AmuletViewModel>();

            this.CreateMap<Fight, FightLogViewModel>();

            this.CreateMap<Chronometer, SideMenuTimersViewComponentModel>();

            this.CreateMap<ResourcePouch, HeroResourcesViewComponentModel>();

            this.CreateMap<Statistics, StatisticsAllViewModel>();

            this.CreateMap<Notification, NotificationViewModel>();
        }
    }
}