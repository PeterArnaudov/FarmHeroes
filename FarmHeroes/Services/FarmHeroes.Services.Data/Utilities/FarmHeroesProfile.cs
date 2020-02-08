namespace Farmheroes.Services.Data.Utilities
{
    using System;

    using AutoMapper;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.BattlefieldModels;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;
    using FarmHeroes.Web.ViewModels.FarmModels;
    using FarmHeroes.Web.ViewModels.FightModels;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using FarmHeroes.Web.ViewModels.MineModels;
    using FarmHeroes.Web.ViewModels.StatisticsModels;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;

    public class FarmHeroesProfile : Profile
    {
        public FarmHeroesProfile()
        {
            this.CreateMap<HeroCreateInputModel, Hero>()
                .ForMember(x => x.Gender, cfg => cfg.MapFrom(x => Enum.Parse<Gender>(x.Gender)))
                .ForMember(x => x.Fraction, cfg => cfg.MapFrom(x => Enum.Parse<Fraction>(x.Fraction)));

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
                .ForMember(x => x.Wins, cfg => cfg.MapFrom(x => x.Statistics.Wins));

            this.CreateMap<Characteristics, CharacteristicsPracticeViewModel>()
                .ForMember(x => x.AttackPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateAttackPrice(x.Attack)))
                .ForMember(x => x.DefensePrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateDefensePrice(x.Defense)))
                .ForMember(x => x.MassPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateMassPrice(x.Mass)))
                .ForMember(x => x.MasteryPrice, cfg => cfg.MapFrom(x => CharacteristicsFormulas.CalculateMasteryPrice(x.Mastery)));

            this.CreateMap<Hero, MineViewModel>()
                .ForMember(x => x.WorkUntil, cfg => cfg.MapFrom(x => x.Chronometer.WorkUntil))
                .ForMember(x => x.DoesWork, cfg => cfg.MapFrom(x => x.WorkStatus == WorkStatus.Mine));

            this.CreateMap<Hero, FarmViewModel>()
                .ForMember(x => x.WorkUntil, cfg => cfg.MapFrom(x => x.Chronometer.WorkUntil))
                .ForMember(x => x.DoesWork, cfg => cfg.MapFrom(x => x.WorkStatus == WorkStatus.Farm))
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

            this.CreateMap<Fight, FightLogViewModel>()
                .ForMember(x => x.AttackerId, cfg => cfg.MapFrom(x => x.HeroFights[1].HeroId))
                .ForMember(x => x.DefenderId, cfg => cfg.MapFrom(x => x.HeroFights[0].HeroId))
                .ForMember(x => x.AttackerName, cfg => cfg.MapFrom(x => x.HeroFights[1].Hero.Name))
                .ForMember(x => x.DefenderName, cfg => cfg.MapFrom(x => x.HeroFights[0].Hero.Name))
                .ForMember(x => x.AttackerAttack, cfg => cfg.MapFrom(x => x.HeroFights[1].Hero.Characteristics.Attack))
                .ForMember(x => x.AttackerDefense, cfg => cfg.MapFrom(x => x.HeroFights[1].Hero.Characteristics.Defense))
                .ForMember(x => x.AttackerMastery, cfg => cfg.MapFrom(x => x.HeroFights[1].Hero.Characteristics.Mastery))
                .ForMember(x => x.AttackerMass, cfg => cfg.MapFrom(x => x.HeroFights[1].Hero.Characteristics.Mass))
                .ForMember(x => x.DefenderAttack, cfg => cfg.MapFrom(x => x.HeroFights[0].Hero.Characteristics.Attack))
                .ForMember(x => x.DefenderDefense, cfg => cfg.MapFrom(x => x.HeroFights[0].Hero.Characteristics.Defense))
                .ForMember(x => x.DefenderMastery, cfg => cfg.MapFrom(x => x.HeroFights[0].Hero.Characteristics.Mastery))
                .ForMember(x => x.DefenderMass, cfg => cfg.MapFrom(x => x.HeroFights[0].Hero.Characteristics.Mass))
                .ForMember(x => x.AttackerHits, cfg => cfg.MapFrom(x => x.HitCollection.ToArray()[0]))
                .ForMember(x => x.DefenderHits, cfg => cfg.MapFrom(x => x.HitCollection.ToArray()[1]));

            this.CreateMap<Chronometer, SideMenuTimersViewComponentModel>();

            this.CreateMap<ResourcePouch, HeroResourcesViewComponentModel>();

            this.CreateMap<Statistics, StatisticsAllViewModel>();
        }
    }
}