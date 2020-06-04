namespace FarmHeroes.Services.Data.Tests
{
    using AutoMapper;
    using Farmheroes.Services.Data.Utilities;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Services.Data.Tests.Common;
    using FarmHeroes.Web.ViewModels.HealthModels;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class HealthServiceTests : IDisposable
    {
        private readonly Hero hero;
        private readonly FarmHeroesDbContext context;

        public HealthServiceTests()
        {
            this.hero = new Hero();
            this.context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeletedAsync().Wait();
        }

        [Fact]
        public async Task GetHealthShouldReturnCorrectHealth()
        {
            // Arrange
            HealthService healthService = this.GetHealthService(this.context);

            // Act
            Health actual = await healthService.GetHealth(this.hero.Id);

            // Assert
            Assert.Equal(this.hero.Health, actual);
        }

        [Fact]
        public async Task GetHealthWithInvalidIdShouldThrowException()
        {
            // Arrange
            HealthService healthService = this.GetHealthService(this.context);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(async () => { await healthService.GetHealth(this.hero.Id + 1); });
        }

        [Fact]
        public async Task GetHealthShouldReturnCorrectHealthWithoutParameter()
        {
            // Arrange
            HealthService healthService = this.GetHealthService(this.context);

            // Act
            Health actual = await healthService.GetHealth();

            // Assert
            Assert.Equal(this.hero.Health, actual);

            await this.context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task IncreaseMaximumHealthShouldIncreaseCorrectlyBasedOnMass()
        {
            // Arrange
            int mass = 15;
            HealthService healthService = this.GetHealthService(this.context);

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
            HealthService healthService = this.GetHealthService(this.context);
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
            HealthService healthService = this.GetHealthService(this.context);
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
            HealthService healthService = this.GetHealthService(this.context);
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
            HealthService healthService = this.GetHealthService(this.context);
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
            HealthService healthService = this.GetHealthService(this.context);
            this.hero.Health.Maximum += 100;

            // Act
            await healthService.HealCurrentHeroToMaximum(0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task ReduceHealthWithIdParameterShouldReduceHealth()
        {
            // Arrange
            Health health = new Health();
            this.hero.Health = health;
            await this.context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(this.context);
            int damage = 10;

            // Act
            await healthService.ReduceHealth(damage, health.Id);
            int expected = health.Maximum - damage;

            // Assert
            Assert.Equal(expected, health.Current);
        }

        [Fact]
        public async Task ReduceHealthWithIdParameterShouldNotReduceHealthBelowOne()
        {
            // Arrange
            Health health = new Health();
            this.hero.Health = health;
            await this.context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(this.context);
            int damage = health.Maximum;

            // Act
            await healthService.ReduceHealth(damage, health.Id);

            // Assert
            Assert.Equal(1, health.Current);
        }

        [Fact]
        public async Task CheckIfDeadWithIdParameterShouldReturnTrueIfCurrentHealthEqualToOne()
        {
            // Arrange
            Health health = new Health() { Current = 1 };
            this.hero.Health = health;
            await this.context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(this.context);

            // Act
            bool actual = await healthService.CheckIfDead(health.Id);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task CheckIfDeadWithIdParameterShouldReturnFalseIfCurrentHealthAboveOne()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Health health = new Health();
            this.hero.Health = health;
            await context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(context);

            // Act
            bool actual = await healthService.CheckIfDead(health.Id);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task CheckIfDeadWithoutIdParameterShouldReturnTrueIfCurrentHealthEqualToOne()
        {
            // Arrange
            this.hero.Health.Current = 1;
            await this.context.SaveChangesAsync();
            HealthService healthService = this.GetHealthService(this.context);

            // Act
            bool actual = await healthService.CheckIfDead(0);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task CheckIfDeadWithoutIdParameterShouldReturnFalseIfCurrentHealthAboveOne()
        {
            // Arrange
            HealthService healthService = this.GetHealthService(this.context);

            // Act
            bool actual = await healthService.CheckIfDead(0);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task UpdateLevelShouldChangeHealthProperly()
        {
            // Arrange
            HealthService healthService = this.GetHealthService(this.context);

            // Act
            await healthService.UpdateHealth(
                new HealthModifyInputModel()
                {
                    HealthCurrent = 30,
                    HealthMaximum = 60,
                    Name = "Name",
                });

            // Assert
            Assert.Equal(30, this.hero.Health.Current);
            Assert.Equal(60, this.hero.Health.Maximum);
        }

        private HealthService GetHealthService(FarmHeroesDbContext context)
        {
            // HeroService
            Mock<IHeroService> heroServiceMock = new Mock<IHeroService>();
            heroServiceMock
                .Setup(x => x.GetHero(0))
                .Returns(Task.FromResult<Hero>(this.hero));
            heroServiceMock
                .Setup(x => x.GetHeroByName("Name"))
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
