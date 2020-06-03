namespace FarmHeroes.Services.Data.Tests
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.DatabaseModels;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Tests.Common;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class LevelServiceTests : IDisposable
    {
        private readonly Hero hero;
        private readonly Hero heroTwo;
        private readonly FarmHeroesDbContext context;

        public LevelServiceTests()
        {
            this.hero = new Hero()
            {
                Level = new Level()
                {
                    CurrentLevel = 5,
                    CurrentExperience = 0,
                    NeededExperience = 33,
                },
            };

            this.heroTwo = new Hero() { Id = 1 };

            this.context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeletedAsync().Wait();
        }

        [Fact]
        public async Task GetCurrentHeroLevelShouldReturnCorrectLevel()
        {
            // Arrange
            LevelService levelService = this.GetLevelService(this.context);

            // Act
            int actual = await levelService.GetCurrentHeroLevel();

            // Assert
            Assert.Equal(this.hero.Level.CurrentLevel, actual);
        }

        [Fact]
        public async Task GiveHeroExperienceWithoutIdShouldGiveExperienceToCurrentHero()
        {
            // Arrange
            LevelService levelService = this.GetLevelService(this.context);

            // Act
            int experience = this.hero.Level.NeededExperience - 10;
            await levelService.GiveHeroExperience(experience);

            // Assert
            Assert.Equal(experience, this.hero.Level.CurrentExperience);
        }

        [Fact]
        public async Task GiveHeroExperienceWithoutIdShouldGiveExperienceToCurrentHeroAndLevelUp()
        {
            // Arrange
            LevelService levelService = this.GetLevelService(this.context);
            await this.SeedLevels(10);

            // Act
            int experience = this.hero.Level.NeededExperience;
            await levelService.GiveHeroExperience(experience);

            // Assert
            Assert.Equal(0, this.hero.Level.CurrentExperience);
            Assert.Equal(6, this.hero.Level.CurrentLevel);
        }

        [Fact]
        public async Task GiveHeroExperienceWithoutIdShouldGiveExperienceToCurrentHeroAndLevelUpAndCarryExtraExperience()
        {
            // Arrange
            LevelService levelService = this.GetLevelService(this.context);
            await this.SeedLevels(10);

            // Act
            int extraExperience = 10;
            int experience = this.hero.Level.NeededExperience + extraExperience;
            await levelService.GiveHeroExperience(experience);

            // Assert
            Assert.Equal(extraExperience, this.hero.Level.CurrentExperience);
            Assert.Equal(6, this.hero.Level.CurrentLevel);
        }

        [Fact]
        public async Task GiveHeroExperienceWithIdShouldGiveExperienceToCorrespondingHero()
        {
            // Arrange
            LevelService levelService = this.GetLevelService(this.context);

            // Act
            int experience = this.heroTwo.Level.NeededExperience - 5;
            await levelService.GiveHeroExperience(experience, this.heroTwo.Id);

            // Assert
            Assert.Equal(experience, this.heroTwo.Level.CurrentExperience);
        }

        [Fact]
        public async Task GiveHeroExperienceWithIdShouldGiveExperienceToCorrespondingHeroAndLevelUp()
        {
            // Arrange
            LevelService levelService = this.GetLevelService(this.context);
            await this.SeedLevels(10);

            // Act
            int experience = this.heroTwo.Level.NeededExperience;
            await levelService.GiveHeroExperience(experience, this.heroTwo.Id);

            // Assert
            Assert.Equal(0, this.heroTwo.Level.CurrentExperience);
            Assert.Equal(2, this.heroTwo.Level.CurrentLevel);
        }

        [Fact]
        public async Task GiveHeroExperienceWithIdShouldGiveExperienceToCorrespondingHeroAndLevelUpAndCarryExtraExperience()
        {
            // Arrange
            LevelService levelService = this.GetLevelService(this.context);
            await this.SeedLevels(10);

            // Act
            int extraExperience = 10;
            int experience = this.heroTwo.Level.NeededExperience + extraExperience;
            await levelService.GiveHeroExperience(experience, this.heroTwo.Id);

            // Assert
            Assert.Equal(extraExperience, this.heroTwo.Level.CurrentExperience);
            Assert.Equal(2, this.heroTwo.Level.CurrentLevel);
        }

        private LevelService GetLevelService(FarmHeroesDbContext context)
        {
            // HeroService
            Mock<IHeroService> heroServiceMock = new Mock<IHeroService>();
            heroServiceMock
                .Setup(x => x.GetHero(0))
                .Returns(Task.FromResult<Hero>(this.hero));
            heroServiceMock
                .Setup(x => x.GetHero(1))
                .Returns(Task.FromResult<Hero>(this.heroTwo));

            // NotificationService
            Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();

            LevelService levelService = new LevelService(this.context, heroServiceMock.Object, notificationServiceMock.Object);

            return levelService;
        }

        private async Task SeedLevels(int levelsCount)
        {
            List<DatabaseLevel> levels = new List<DatabaseLevel>();
            int neededExperience = 20;
            double modifier = 1.15;

            for (int i = 1; i <= levelsCount; i++)
            {
                levels.Add(new DatabaseLevel()
                {
                    Level = i,
                    NeededExperience = neededExperience,
                });

                neededExperience = (int)(neededExperience * modifier);
            }

            await this.context.DatabaseLevels.AddRangeAsync(levels);
            await this.context.SaveChangesAsync();
        }
    }
}
