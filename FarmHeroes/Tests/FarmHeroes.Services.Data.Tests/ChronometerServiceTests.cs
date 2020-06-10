namespace FarmHeroes.Services.Data.Tests
{
    using AutoMapper;
    using Farmheroes.Services.Data.Utilities;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.DatabaseModels;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Tests.Common;
    using FarmHeroes.Web.ViewModels.ChronometerModels;
    using FarmHeroes.Web.ViewModels.LevelModels;
    using FarmHeroes.Web.ViewModels.ViewComponentsModels;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ChronometerServiceTests : IDisposable
    {
        private readonly Hero hero;
        private readonly Hero heroTwo;
        private readonly FarmHeroesDbContext context;
        private readonly ChronometerService chronometerService;

        public ChronometerServiceTests()
        {
            this.hero = new Hero();
            this.heroTwo = new Hero();

            this.context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();

            this.chronometerService = this.GetChronometerService();
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeletedAsync().Wait();
        }

        [Fact]
        public async Task GetChronometerWithoutIdParameterShouldReturnCorrectChronometer()
        {
            // Act
            Chronometer actual = await this.chronometerService.GetChronometer(0);

            // Assert
            Assert.Equal(this.hero.Chronometer, actual);
        }

        [Fact]
        public async Task GetChronometerWithIdParameterShouldReturnCorrectChronometer()
        {
            // Arrange
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();

            // Act
            Chronometer actual = await this.chronometerService.GetChronometer(this.heroTwo.Id);

            // Assert
            Assert.Equal(this.heroTwo.Chronometer, actual);
        }

        [Fact]
        public async Task GetChronometerWithInvalidIdShouldThrowException()
        {
            // Act
            await Assert.ThrowsAsync<NullReferenceException>(async () => await this.chronometerService.GetChronometer(5));
        }

        [Fact]
        public async Task GetCurrentHeroChronometerViewModelShouldReturnCorrectViewModel()
        {
            // Act
            SideMenuTimersViewComponentModel viewModel = await this.chronometerService.GetCurrentHeroChronometerViewModel<SideMenuTimersViewComponentModel>();

            // Assert
            Assert.Equal(this.hero.Chronometer.WorkUntil, viewModel.WorkUntil);
            Assert.Equal(this.hero.Chronometer.CannotBeAttackedUntil, viewModel.CannotBeAttackedUntil);
            Assert.Equal(this.hero.Chronometer.CannotAttackHeroUntil, viewModel.CannotAttackHeroUntil);
        }

        [Fact]
        public async Task SetWorkUntilShouldSetWorkUntilPropertyIfNotWorking()
        {
            // Arrange
            int seconds = 130;
            DateTime dateTime = DateTime.UtcNow.AddSeconds(seconds);

            // Act
            await this.chronometerService.SetWorkUntil(seconds, WorkStatus.Mine);

            // Assert
            Assert.True(this.hero.Chronometer.WorkUntil.HasValue);
            Assert.Equal(dateTime.Hour, this.hero.Chronometer.WorkUntil.Value.Hour);
            Assert.Equal(dateTime.Minute, this.hero.Chronometer.WorkUntil.Value.Minute);
            Assert.Equal(dateTime.Second, this.hero.Chronometer.WorkUntil.Value.Second);
        }

        [Fact]
        public async Task SetWorkUntilShouldSetWorkStatusPropertyIfNotWorking()
        {
            // Arrange
            int seconds = 130;

            // Act
            await this.chronometerService.SetWorkUntil(seconds, WorkStatus.Mine);

            // Assert
            Assert.Equal(WorkStatus.Mine, this.hero.WorkStatus);
        }

        [Fact]
        public async Task SetWorkUntilShouldThrowExceptionIfWorking()
        {
            // Arrange
            int seconds = 130;
            this.hero.WorkStatus = WorkStatus.Farm;
            this.hero.Chronometer.WorkUntil = DateTime.UtcNow.AddHours(1);
            await this.context.SaveChangesAsync();

            // Act
            await Assert.ThrowsAsync<FarmHeroesException>(async () => await this.chronometerService.SetWorkUntil(seconds, WorkStatus.Mine));
        }

        [Fact]
        public async Task NullifyWorkUntilShouldSetWorkStatusToIdleIfNotPatrolling()
        {
            // Arrange
            this.hero.WorkStatus = WorkStatus.Farm;
            this.hero.Chronometer.WorkUntil = DateTime.UtcNow.AddHours(1);
            await this.context.SaveChangesAsync();

            // Act
            await this.chronometerService.NullifyWorkUntil();

            // Assert
            Assert.Equal(WorkStatus.Idle, this.hero.WorkStatus);
        }

        [Fact]
        public async Task NullifyWorkUntilShouldSetWorkUntilToNullIfNotPatrolling()
        {
            // Arrange
            this.hero.WorkStatus = WorkStatus.Farm;
            this.hero.Chronometer.WorkUntil = DateTime.UtcNow.AddHours(1);
            await this.context.SaveChangesAsync();

            // Act
            await this.chronometerService.NullifyWorkUntil();

            // Assert
            Assert.Null(this.hero.Chronometer.WorkUntil);
        }

        [Fact]
        public async Task NullifyWorkUntilShouldThrowExceptionIfPatrolling()
        {
            // Arrange
            this.hero.WorkStatus = WorkStatus.Battlefield;
            this.hero.Chronometer.WorkUntil = DateTime.UtcNow.AddMinutes(10);
            await this.context.SaveChangesAsync();

            // Act
            await Assert.ThrowsAsync<FarmHeroesException>(async () => await this.chronometerService.NullifyWorkUntil());
        }

        [Fact]
        public async Task SetCannotAttackHeroUntilWithoutIdParameterShouldSetProperty()
        {
            // Arrange
            int seconds = 600;

            // Act
            await this.chronometerService.SetCannotAttackHeroUntil(seconds);

            // Assert
            Assert.Equal(DateTime.UtcNow.AddSeconds(seconds).Minute, this.hero.Chronometer.CannotAttackHeroUntil.Value.Minute);
        }

        [Fact]
        public async Task SetCannotAttackHeroUntilWithIdParameterShouldSetProperty()
        {
            // Arrange
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();
            int seconds = 600;

            // Act
            await this.chronometerService.SetCannotAttackHeroUntil(seconds, this.heroTwo.Id);

            // Assert
            Assert.Equal(DateTime.UtcNow.AddSeconds(seconds).Minute, this.heroTwo.Chronometer.CannotAttackHeroUntil.Value.Minute);
        }

        [Fact]
        public async Task SetCannotAttackMonsterUntilWithoutIdParameterShouldSetProperty()
        {
            // Arrange
            int seconds = 600;

            // Act
            await this.chronometerService.SetCannotAttackMonsterUntil(seconds);

            // Assert
            Assert.Equal(DateTime.UtcNow.AddSeconds(seconds).Minute, this.hero.Chronometer.CannotAttackMonsterUntil.Value.Minute);
        }

        [Fact]
        public async Task SetCannotAttackMonsterUntilWithIdParameterShouldSetProperty()
        {
            // Arrange
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();
            int seconds = 600;

            // Act
            await this.chronometerService.SetCannotAttackMonsterUntil(seconds, this.heroTwo.Id);

            // Assert
            Assert.Equal(DateTime.UtcNow.AddSeconds(seconds).Minute, this.heroTwo.Chronometer.CannotAttackMonsterUntil.Value.Minute);
        }

        [Fact]
        public async Task SetCannotBeAttackedUntilWithoutIdParameterShouldSetProperty()
        {
            // Arrange
            int seconds = 600;

            // Act
            await this.chronometerService.SetCannotBeAttackedUntil(seconds);

            // Assert
            Assert.Equal(DateTime.UtcNow.AddSeconds(seconds).Minute, this.hero.Chronometer.CannotBeAttackedUntil.Value.Minute);
        }

        [Fact]
        public async Task SetCannotBeAttackedUntilWithIdParameterShouldSetProperty()
        {
            // Arrange
            await this.context.Heroes.AddAsync(this.heroTwo);
            await this.context.SaveChangesAsync();
            int seconds = 600;

            // Act
            await this.chronometerService.SetCannotBeAttackedUntil(seconds, this.heroTwo.Id);

            // Assert
            Assert.Equal(DateTime.UtcNow.AddSeconds(seconds).Minute, this.heroTwo.Chronometer.CannotBeAttackedUntil.Value.Minute);
        }

        [Fact]
        public async Task UpdateChronometerShouldChangeChronometerProperly()
        {
            // Arrange
            ChronometerModifyInputModel inputModel = new ChronometerModifyInputModel()
            {
                Name = "Name",
                ChronometerWorkUntil = DateTime.UtcNow,
                ChronometerCannotAttackHeroUntil = DateTime.UtcNow,
                ChronometerCannotAttackMonsterUntil = DateTime.UtcNow,
                ChronometerCannotBeAttackedUntil = DateTime.UtcNow,
            };

            // Act
            await this.chronometerService.UpdateChronometer(inputModel);

            // Assert
            Assert.Equal(inputModel.ChronometerWorkUntil, this.hero.Chronometer.WorkUntil);
            Assert.Equal(inputModel.ChronometerCannotAttackHeroUntil, this.hero.Chronometer.CannotAttackHeroUntil);
            Assert.Equal(inputModel.ChronometerCannotAttackMonsterUntil, this.hero.Chronometer.CannotAttackMonsterUntil);
            Assert.Equal(inputModel.ChronometerCannotBeAttackedUntil, this.hero.Chronometer.CannotBeAttackedUntil);
        }

        private ChronometerService GetChronometerService()
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

            // AmuletBagService
            Mock<IAmuletBagService> amuletBagServiceMock = new Mock<IAmuletBagService>();

            // AutoMapper
            var farmHeroesProfile = new FarmHeroesProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(farmHeroesProfile));
            IMapper mapper = new Mapper(configuration);

            ChronometerService chronometerService = new ChronometerService(this.context, heroServiceMock.Object, mapper, amuletBagServiceMock.Object);

            return chronometerService;
        }
    }
}