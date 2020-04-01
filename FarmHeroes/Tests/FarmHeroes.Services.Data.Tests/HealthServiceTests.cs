namespace FarmHeroes.Services.Data.Tests
{
    using AutoMapper;
    using Farmheroes.Services.Data.Utilities;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Services.Data.Tests.Common;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class HealthServiceTests
    {
        private readonly Hero hero = new Hero();

        [Fact]
        public async Task GetHealthByIdShouldReturnCorrectHealth()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Health health = new Health();
            await context.Healths.AddAsync(health);
            await context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(context);

            // Act
            Health actual = await healthService.GetHealthById(health.Id);

            // Assert
            Assert.Equal(health, actual);
        }

        [Fact]
        public async Task GetHealthByIdWithInvalidIdShouldReturnNull()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Health health = new Health();
            await context.Healths.AddAsync(health);
            await context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(context);

            // Act
            Health actual = await healthService.GetHealthById(health.Id + 1);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetCurrentHealthShouldReturnCorrectHealth()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HealthService healthService = this.GetHealthService(context);

            // Act
            Health actual = await healthService.GetCurrentHeroHealth();

            // Assert
            Assert.Equal(this.hero.Health, actual);
        }

        [Fact]
        public async Task IncreaseMaximumHealthShouldIncreaseCorrectlyBasedOnMass()
        {
            // Arrange
            int mass = 15;
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Health health = new Health();
            await context.Healths.AddAsync(health);
            await context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(context);

            // Act
            await healthService.IncreaseMaximumHealth(mass);
            int expected = HealthFormulas.CalculateMaximumHealth(mass);

            // Assert
            Assert.Equal(expected, this.hero.Health.Maximum);
        }

        [Fact]
        public async Task HealCurrentHeroShouldWorkProperly()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HealthService healthService = this.GetHealthService(context);
            this.hero.Health.Maximum = 1000;
            int healAmount = 100;

            // Act
            await healthService.HealCurrentHero(healAmount, 0);

            // Assert
            Assert.Equal(50 + healAmount, this.hero.Health.Current);
        }

        [Fact]
        public async Task HealCurrentHeroShouldNotIncreaseCurrentHealthAboveMaximumHealthWithInitialValues()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HealthService healthService = this.GetHealthService(context);
            int healAmount = 100;

            // Act
            await healthService.HealCurrentHero(healAmount, 0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task HealCurrentHeroShouldNotIncreaseCurrentHealthAboveMaximumHealth()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HealthService healthService = this.GetHealthService(context);
            this.hero.Health.Maximum = 100;
            int healAmount = 100;

            // Act
            await healthService.HealCurrentHero(healAmount, 0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task HealCurrentHeroToMaximumShouldHealExactlyToTheMaximumWithInitialValues()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HealthService healthService = this.GetHealthService(context);
            this.hero.Health.Current -= 30;

            // Act
            await healthService.HealCurrentHeroToMaximum(0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task HealCurrentHeroToMaximumShouldHealExactlyToTheMaximum()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HealthService healthService = this.GetHealthService(context);
            this.hero.Health.Maximum += 100;

            // Act
            await healthService.HealCurrentHeroToMaximum(0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task ReduceHealthByIdShouldReduceHealth()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Health health = new Health();
            await context.Healths.AddAsync(health);
            await context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(context);
            int damage = 10;

            // Act
            await healthService.ReduceHealthById(health.Id, damage);
            int expected = health.Maximum - damage;

            // Assert
            Assert.Equal(expected, health.Current);
        }

        [Fact]
        public async Task ReduceHealthByIdShouldNotReduceHealthBelowOne()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Health health = new Health();
            await context.Healths.AddAsync(health);
            await context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(context);
            int damage = health.Maximum;

            // Act
            await healthService.ReduceHealthById(health.Id, damage);

            // Assert
            Assert.Equal(1, health.Current);
        }

        [Fact]
        public async Task CheckIfDeadShouldReturnTrueIfCurrentHealthEqualToOne()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Health health = new Health() { Current = 1 };
            await context.Healths.AddAsync(health);
            await context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(context);

            // Act
            bool actual = await healthService.CheckIfDead(health.Id);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task CheckIfDeadShouldReturnFalseIfCurrentHealthAboveOne()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Health health = new Health();
            await context.Healths.AddAsync(health);
            await context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(context);

            // Act
            bool actual = await healthService.CheckIfDead(health.Id);

            // Assert
            Assert.False(actual);
        }

        private HealthService GetHealthService(FarmHeroesDbContext context)
        {
            // HeroService
            Mock<IHeroService> heroServiceMock = new Mock<IHeroService>();
            heroServiceMock
                .Setup(x => x.GetCurrentHero())
                .Returns(Task.FromResult<Hero>(this.hero));

            // ResourcePouchService
            Mock<IResourcePouchService> resourcePouchServiceMock = new Mock<IResourcePouchService>();

            // AutoMapper
            var farmHeroesProfile = new FarmHeroesProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(farmHeroesProfile));
            IMapper mapper = new Mapper(configuration);

            HealthService healthService = new HealthService(mapper, heroServiceMock.Object, resourcePouchServiceMock.Object, context);

            return healthService;
        }
    }
}
