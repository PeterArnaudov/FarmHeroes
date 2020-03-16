namespace FarmHeroes.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using FarmHeroes.Data.Common.Models;
    using FarmHeroes.Data.Models;
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Data.Models.MappingModels;
    using FarmHeroes.Data.Models.MonsterModels;
    using FarmHeroes.Data.Models.NotificationModels.HeroModels;
    using FarmHeroes.Data.Models.ShopModels;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FarmHeroesDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(FarmHeroesDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public FarmHeroesDbContext(DbContextOptions<FarmHeroesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hero> Heroes { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<Level> Levels { get; set; }

        public virtual DbSet<Health> Healths { get; set; }

        public virtual DbSet<Inventory> Inventories { get; set; }

        public virtual DbSet<EquippedSet> EquippedSets { get; set; }

        public virtual DbSet<ResourcePouch> ResourcePouches { get; set; }

        public virtual DbSet<Statistics> Statistics { get; set; }

        public virtual DbSet<Chronometer> Chronometers { get; set; }

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

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}

