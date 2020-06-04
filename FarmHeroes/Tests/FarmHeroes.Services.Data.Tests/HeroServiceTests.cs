namespace FarmHeroes.Services.Data.Tests
{
    using AutoMapper;
    using Farmheroes.Services.Data.Utilities;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Tests.Common;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class HeroServiceTests : IDisposable
    {
        private const string UserName = "TestUser";
        private readonly FarmHeroesDbContext context;

        public HeroServiceTests()
        {
            this.context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeletedAsync().Wait();
        }

        [Fact]
        public async Task CreateHeroShouldCreateHeroInDatabase()
        {
            // Arrange
            HeroCreateInputModel inputModel = new HeroCreateInputModel() { Fraction = Fraction.Sheep, Gender = Gender.Male };
            HeroService heroService = this.GetHeroServiceInitialValues();

            // Act
            await heroService.CreateHero(inputModel);

            // Assert
            Assert.True(this.context.Heroes.Any());
        }

        [Fact]
        public async Task CreateHeroShouldCreateHeroWithProperName()
        {
            // Arrange
            HeroCreateInputModel inputModel = new HeroCreateInputModel() { Fraction = Fraction.Sheep, Gender = Gender.Male };
            HeroService heroService = this.GetHeroServiceInitialValues();

            // Act
            await heroService.CreateHero(inputModel);
            Hero hero = this.context.Heroes.First();

            // Assert
            Assert.Equal(UserName, hero.Name);
        }

        [Fact]
        public async Task CreateHeroShouldIgnoreNameInInputModel()
        {
            // Arrange
            HeroCreateInputModel inputModel = new HeroCreateInputModel() { Fraction = Fraction.Sheep, Gender = Gender.Male, Name = "Name" };
            HeroService heroService = this.GetHeroServiceInitialValues();

            // Act
            await heroService.CreateHero(inputModel);
            Hero hero = this.context.Heroes.First();

            // Assert
            Assert.Equal(UserName, hero.Name);
        }

        [Fact]
        public async Task CreateHeroShouldUseGenderAndFractionFromInputModel()
        {
            // Arrange
            Fraction fraction = Fraction.Pig;
            Gender gender = Gender.Female;
            HeroCreateInputModel inputModel = new HeroCreateInputModel() { Fraction = fraction, Gender = gender };
            HeroService heroService = this.GetHeroServiceInitialValues();

            // Act
            await heroService.CreateHero(inputModel);

            // Assert
            Assert.True(this.context.Heroes.Any(x => x.Fraction == fraction && x.Gender == gender));
        }

        [Fact]
        public async Task GetHeroWithoutParameterShouldReturnCorrectHero()
        {
            // Arrange
            HeroService heroService = this.GetHeroServiceInitialValues();

            // Act
            Hero hero = await heroService.GetHero();

            // Assert
            Assert.Equal(UserName, hero.Name);
        }

        [Fact]
        public async Task GetHeroWithParameterShouldReturnCorrectHero()
        {
            // Arrange
            HeroService heroService = this.GetHeroServiceInitialValues();
            Hero heroOne = new Hero() { Name = "HeroOne" };
            Hero heroTwo = new Hero() { Name = "HeroTwo" };
            await this.context.Heroes.AddAsync(heroOne);
            await this.context.Heroes.AddAsync(heroTwo);

            // Act
            Hero heroById = await heroService.GetHero(heroTwo.Id);

            // Assert
            Assert.Equal(heroTwo, heroById);
        }

        [Fact]
        public async Task GetHeroWithInvalidIdShouldReturnNull()
        {
            // Arrange
            HeroService heroService = this.GetHeroServiceInitialValues();
            Hero hero = new Hero() { Name = "HeroOne" };
            await this.context.Heroes.AddAsync(hero);

            // Act
            Hero heroById = await heroService.GetHero(hero.Id + 1);

            // Assert
            Assert.Null(heroById);
        }

        [Fact]
        public async Task GetHeroByNameWithInvalidNameShouldReturnNull()
        {
            // Arrange
            HeroService heroService = this.GetHeroServiceInitialValues();
            Hero hero = new Hero() { Name = "HeroOne" };
            await this.context.Heroes.AddAsync(hero);

            // Act
            Hero heroByName = await heroService.GetHeroByName("Invalid name");

            // Assert
            Assert.Null(heroByName);
        }

        [Fact]
        public async Task ValidateCurrentHeroLocationShouldNotThrowExceptionIfWorkStatusIdle()
        {
            // Arrange
            HeroService heroService = this.GetHeroServiceInitialValues();

            // Act
            await heroService.ValidateCurrentHeroLocation(WorkStatus.Farm);
        }

        [Fact]
        public async Task ValidateCurrentHeroLocationShouldNotThrowExceptionIfWorkStatusSame()
        {
            // Arrange
            HeroService heroService = this.GetHeroService();

            // Act
            await heroService.ValidateCurrentHeroLocation(WorkStatus.Farm);
        }

        [Fact]
        public async Task ValidateCurrentHeroLocationShouldThrowExceptionIfWorkStatusDifferent()
        {
            // Arrange
            HeroService heroService = this.GetHeroService();

            // Act
            await Assert.ThrowsAsync<FarmHeroesException>(async () => await heroService.ValidateCurrentHeroLocation(WorkStatus.Mine));
        }

        private HeroService GetHeroServiceInitialValues()
        {
            // UserService
            Mock<IUserService> userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(x => x.GetApplicationUser())
                .Returns(Task.FromResult<ApplicationUser>(new ApplicationUser()
                {
                    UserName = UserName,
                    Hero = new Hero()
                    {
                        Name = UserName,
                    },
                }));

            // AutoMapper
            var farmHeroesProfile = new FarmHeroesProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(farmHeroesProfile));
            IMapper mapper = new Mapper(configuration);

            HeroService heroService = new HeroService(this.context, mapper, userServiceMock.Object);

            return heroService;
        }

        private HeroService GetHeroService()
        {
            // UserService
            Mock<IUserService> userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(x => x.GetApplicationUser())
                .Returns(Task.FromResult<ApplicationUser>(new ApplicationUser()
                {
                    UserName = UserName,
                    Hero = new Hero()
                    {
                        Name = UserName,
                        WorkStatus = WorkStatus.Farm,
                    },
                }));

            // AutoMapper
            var farmHeroesProfile = new FarmHeroesProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(farmHeroesProfile));
            IMapper mapper = new Mapper(configuration);

            HeroService heroService = new HeroService(this.context, mapper, userServiceMock.Object);

            return heroService;
        }
    }
}
