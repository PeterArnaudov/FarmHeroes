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
        private readonly Hero heroTwo;
        private readonly FarmHeroesDbContext context;
        private readonly HealthService healthService;

        public HealthServiceTests()
        {
            this.hero = new Hero();
            this.heroTwo = new Hero();

            this.context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            this.healthService = this.GetHealthService();
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeletedAsync().Wait();
        }

        [Fact]
        public async Task GetHealthWithoutIdParameterShouldReturnCorrectHealth()
        {
            // Act
            Health actual = await this.healthService.GetHealth();

            // Assert
            Assert.Equal(this.hero.Health, actual);
        }

        [Fact]
        public async Task GetHealthWithIdParameterShouldReturnCorrectHealth()
        {
            // Arrange
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();

            // Act
            Health actual = await this.healthService.GetHealth(this.heroTwo.Id);

            // Assert
            Assert.Equal(this.heroTwo.Health, actual);
        }

        [Fact]
        public async Task GetHealthWithInvalidIdShouldThrowException()
        {
            // Act
            await Assert.ThrowsAsync<NullReferenceException>(async () => { await this.healthService.GetHealth(this.hero.Id + 5); });
        }

        [Fact]
        public async Task IncreaseMaximumHealthShouldIncreaseCorrectlyBasedOnMass()
        {
            // Arrange
            int mass = 15;

            // Act
            await this.healthService.IncreaseMaximumHealth(mass);
            int expected = HealthFormulas.CalculateMaximumHealth(mass);

            // Assert
            Assert.Equal(expected, this.hero.Health.Maximum);
        }

        [Fact]
        public async Task HealCurrentHeroShouldWorkProperly()
        {
            // Arrange
            this.hero.Health.Maximum = 1000;
            int healAmount = 100;

            // Act
            await this.healthService.HealCurrentHero(healAmount, 0);

            // Assert
            Assert.Equal(50 + healAmount, this.hero.Health.Current);
        }

        [Fact]
        public async Task HealCurrentHeroShouldNotIncreaseCurrentHealthAboveMaximumHealthWithInitialValues()
        {
            // Arrange
            int healAmount = 100;

            // Act
            await this.healthService.HealCurrentHero(healAmount, 0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task HealCurrentHeroShouldNotIncreaseCurrentHealthAboveMaximumHealth()
        {
            // Arrange
            this.hero.Health.Maximum = 100;
            int healAmount = 100;

            // Act
            await this.healthService.HealCurrentHero(healAmount, 0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task HealCurrentHeroToMaximumShouldHealExactlyToTheMaximumWithInitialValues()
        {
            // Arrange
            this.hero.Health.Current -= 30;

            // Act
            await this.healthService.HealCurrentHeroToMaximum(0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task HealCurrentHeroToMaximumShouldHealExactlyToTheMaximum()
        {
            // Arrange
            this.hero.Health.Maximum += 100;

            // Act
            await this.healthService.HealCurrentHeroToMaximum(0);

            // Assert
            Assert.Equal(this.hero.Health.Maximum, this.hero.Health.Current);
        }

        [Fact]
        public async Task ReduceHealthWithIdParameterShouldReduceHealth()
        {
            // Arrange
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();
            int damage = 10;

            // Act
            await this.healthService.ReduceHealth(damage, this.heroTwo.Id);
            int expected = this.heroTwo.Health.Maximum - damage;

            // Assert
            Assert.Equal(expected, this.heroTwo.Health.Current);
        }

        [Fact]
        public async Task ReduceHealthWithIdParameterShouldNotReduceHealthBelowOne()
        {
            // Arrange
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();
            int damage = this.heroTwo.Health.Maximum;

            // Act
            await this.healthService.ReduceHealth(damage, this.heroTwo.Id);

            // Assert
            Assert.Equal(1, this.heroTwo.Health.Current);
        }

        [Fact]
        public async Task CheckIfDeadWithIdParameterShouldReturnTrueIfCurrentHealthEqualToOne()
        {
            // Arrange
            this.heroTwo.Health.Current = 1;
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();

            // Act
            bool actual = await this.healthService.CheckIfDead(this.heroTwo.Id);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task CheckIfDeadWithIdParameterShouldReturnFalseIfCurrentHealthAboveOne()
        {
            // Arrange
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();

            // Act
            bool actual = await this.healthService.CheckIfDead(this.heroTwo.Id);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task CheckIfDeadWithoutIdParameterShouldReturnTrueIfCurrentHealthEqualToOne()
        {
            // Arrange
            this.hero.Health.Current = 1;
            await this.context.SaveChangesAsync();

            // Act
            bool actual = await this.healthService.CheckIfDead(0);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task CheckIfDeadWithoutIdParameterShouldReturnFalseIfCurrentHealthAboveOne()
        {
            // Act
            bool actual = await this.healthService.CheckIfDead(0);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task UpdateLevelShouldChangeHealthProperly()
        {
            // Act
            await this.healthService.UpdateHealth(
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

        private HealthService GetHealthService()
        {
            // HeroService
            Mock<IHeroService> heroServiceMock = new Mock<IHeroService>();
            heroServiceMock
                .Setup(x => x.GetHero(0))
                .Returns(Task.FromResult<Hero>(this.hero));
            heroServiceMock
                .Setup(x => x.GetHero(1))
                .Returns(Task.FromResult<Hero>(this.heroTwo));
            heroServiceMock
                .Setup(x => x.GetHeroByName("Name"))
                .Returns(Task.FromResult<Hero>(this.hero));

            // ResourcePouchService
            Mock<IResourcePouchService> resourcePouchServiceMock = new Mock<IResourcePouchService>();

            // AutoMapper
            var farmHeroesProfile = new FarmHeroesProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(farmHeroesProfile));
            IMapper mapper = new Mapper(configuration);

            HealthService healthService = new HealthService(mapper, heroServiceMock.Object, resourcePouchServiceMock.Object, this.context);

            return healthService;
        }
    }
}
