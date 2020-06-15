namespace FarmHeroes.Data.Models.HeroModels
{
    using System;
    using System.Collections.Generic;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.MappingModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;

    public class Hero
    {
        public Hero()
        {
            this.Characteristics = new Characteristics();
            this.EquippedSet = new EquippedSet();
            this.Health = new Health();
            this.Inventory = new Inventory();
            this.Level = new Level();
            this.ResourcePouch = new ResourcePouch();
            this.Statistics = new Statistics();
            this.Chronometer = new Chronometer();
            this.DailyLimits = new DailyLimits();
            this.AmuletBag = new AmuletBag();
            this.DungeonInformation = new DungeonInformation();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string AvatarUrl { get; set; }

        public Fraction Fraction { get; set; }

        public Gender Gender { get; set; }

        public WorkStatus WorkStatus { get; set; }

        public int LevelId { get; set; }

        public virtual Level Level { get; set; }

        public int HealthId { get; set; }

        public virtual Health Health { get; set; }

        public int ResourcePouchId { get; set; }

        public virtual ResourcePouch ResourcePouch { get; set; }

        public int CharacteristicsId { get; set; }

        public virtual Characteristics Characteristics { get; set; }

        public int EquippedSetId { get; set; }

        public virtual EquippedSet EquippedSet { get; set; }

        public int InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }

        public int StatisticsId { get; set; }

        public virtual Statistics Statistics { get; set; }

        public int ChronometerId { get; set; }

        public virtual Chronometer Chronometer { get; set; }

        public int DailyLimitsId { get; set; }

        public virtual DailyLimits DailyLimits { get; set; }

        public int AmuletBagId { get; set; }

        public virtual AmuletBag AmuletBag { get; set; }

        public int DungeonInformationId { get; set; }

        public virtual DungeonInformation DungeonInformation { get; set; }

        public virtual List<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual List<HeroFight> HeroFights { get; set; } = new List<HeroFight>();
    }
}
