namespace FarmHeroes.Data
{
    using System.Reflection;

    using FarmHeroes.Data.Models;
    using FarmHeroes.Data.Models.Chat;
    using FarmHeroes.Data.Models.DatabaseModels;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MappingModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Data.Models.News;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Data.Models.ShopModels;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FarmHeroesDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public FarmHeroesDbContext(DbContextOptions<FarmHeroesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hero> Heroes { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<DatabaseLevel> DatabaseLevels { get; set; }

        public virtual DbSet<Level> Levels { get; set; }

        public virtual DbSet<Health> Healths { get; set; }

        public virtual DbSet<Inventory> Inventories { get; set; }

        public virtual DbSet<EquippedSet> EquippedSets { get; set; }

        public virtual DbSet<ResourcePouch> ResourcePouches { get; set; }

        public virtual DbSet<Statistics> Statistics { get; set; }

        public virtual DbSet<Chronometer> Chronometers { get; set; }

        public virtual DbSet<DailyLimits> DailyLimits { get; set; }

        public virtual DbSet<Characteristics> Characteristics { get; set; }

        public virtual DbSet<HeroEquipment> HeroEquipments { get; set; }

        public virtual DbSet<ShopEquipment> ShopEquipments { get; set; }

        public virtual DbSet<HeroAmulet> HeroAmulets { get; set; }

        public virtual DbSet<ShopAmulet> ShopAmulets { get; set; }

        public virtual DbSet<HeroBonus> HeroBonuses { get; set; }

        public virtual DbSet<ShopBonus> ShopBonuses { get; set; }

        public virtual DbSet<Monster> Monsters { get; set; }

        public virtual DbSet<Fight> Fights { get; set; }

        public virtual DbSet<HeroFight> HeroFights { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<DungeonInformation> DungeonInformations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}