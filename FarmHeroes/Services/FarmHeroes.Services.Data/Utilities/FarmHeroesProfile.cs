namespace Farmheroes.Services.Data.Utilities
{
    using System;
    using System.Linq;
    using AutoMapper;
    using FarmHeroes.Data.Models.Chat;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Data.Models.News;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Data.Models.ShopModels;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.AmuletBagModels;
    using FarmHeroes.Web.ViewModels.BattlefieldModels;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;
    using FarmHeroes.Web.ViewModels.ChatModels;
    using FarmHeroes.Web.ViewModels.ChronometerModels;
    using FarmHeroes.Web.ViewModels.DungeonModels;
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using FarmHeroes.Web.ViewModels.FarmModels;
    using FarmHeroes.Web.ViewModels.FightModels;
    using FarmHeroes.Web.ViewModels.HealthModels;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using FarmHeroes.Web.ViewModels.HutModels;
    using FarmHeroes.Web.ViewModels.InventoryModels;
    using FarmHeroes.Web.ViewModels.LevelModels;
    using FarmHeroes.Web.ViewModels.MineModels;
    using FarmHeroes.Web.ViewModels.MonsterModels;
    using FarmHeroes.Web.ViewModels.NewsModels;
    using FarmHeroes.Web.ViewModels.NotificationModels;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
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
                .ForMember(x => x.EquippedSetHelmet, cfg => cfg.MapFrom(x => x.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Helmet)))
                .ForMember(x => x.EquippedSetArmor, cfg => cfg.MapFrom(x => x.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Armor)))
                .ForMember(x => x.EquippedSetWeapon, cfg => cfg.MapFrom(x => x.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Weapon)))
                .ForMember(x => x.EquippedSetShield, cfg => cfg.MapFrom(x => x.EquippedSet.Equipped.Find(x => x.Type == EquipmentType.Shield)));

            this.CreateMap<Hero, HeroViewModel>();

            this.CreateMap<Characteristics, CharacteristicsPracticeViewModel>()
                .ForMember(x => x.AttackPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateAttackPrice(x.Attack)))
                .ForMember(x => x.DefensePrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateDefensePrice(x.Defense)))
                .ForMember(x => x.MassPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateMassPrice(x.Mass)))
                .ForMember(x => x.MasteryPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateMasteryPrice(x.Mastery)))
                .ForMember(x => x.DexterityPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateDexterityPrice(x.Dexterity)));

            this.CreateMap<Hero, MineViewModel>();

            this.CreateMap<Hero, FarmViewModel>()
                .ForMember(x => x.Salary, cfg => cfg.MapFrom(x => FarmFormulas.CalculateFarmSalaryPerHour(x.Level.CurrentLevel)));

            this.CreateMap<Hero, BattlefieldViewModel>();

            this.CreateMap<Health, HeroHealthViewComponentModel>();

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

            this.CreateMap<ShopBonus[], HutBonusesViewModel>()
                .ForMember(x => x.Bonuses, cfg => cfg.MapFrom(x => x));

            this.CreateMap<Inventory, SmithViewModel>();

            this.CreateMap<Hero, BattlefieldOpponentViewModel>();

            this.CreateMap<Hero, HeroModifyBasicInfoInputModel>();

            this.CreateMap<Hero, LevelModifyInputModel>();

            this.CreateMap<Hero, HealthModifyInputModel>();

            this.CreateMap<Hero, ResourcePouchModifyInputModel>();

            this.CreateMap<Hero, CharacteristicsModifyInputModel>();

            this.CreateMap<Hero, ChronometerModifyInputModel>();

            this.CreateMap<ShopEquipment, EquipmentViewModel>();

            this.CreateMap<HeroEquipment, EquipmentViewModel>();

            this.CreateMap<ShopAmulet, AmuletViewModel>();

            this.CreateMap<HeroAmulet, AmuletViewModel>();

            this.CreateMap<HeroBonus, BonusViewModel>();

            this.CreateMap<Fight, FightLogViewModel>();

            this.CreateMap<Chronometer, SideMenuTimersViewComponentModel>();

            this.CreateMap<Inventory, SideMenuAmuletsViewComponentModel>();

            this.CreateMap<Inventory, SideMenuBonusesViewComponentModel>();

            this.CreateMap<ResourcePouch, HeroResourcesViewComponentModel>();

            this.CreateMap<ResourcePouch, SideMenuResourcesViewComponentModel>();

            this.CreateMap<Statistics, StatisticsAllViewModel>();

            this.CreateMap<Notification, NotificationViewModel>();

            this.CreateMap<MonsterInputModel, Monster>();

            this.CreateMap<Monster, MonsterInputModel>();

            this.CreateMap<AmuletBag, AmuletBagViewModel>();

            this.CreateMap<HeroAmulet, AmuletSelectViewModel>();

            this.CreateMap<Hero, DungeonIndexViewModel>();

            this.CreateMap<Hero, DungeonWalkingViewModel>();

            this.CreateMap<NewsInputModel, News>();

            this.CreateMap<News, NewsInputModel>();

            this.CreateMap<News, NewsListViewModel>();

            this.CreateMap<News, NewsDetailsViewModel>();
        }
    }
}