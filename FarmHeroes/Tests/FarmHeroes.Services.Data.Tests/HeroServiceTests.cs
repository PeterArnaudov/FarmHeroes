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
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Xunit;

    public class HeroServiceTests
    {
        private const string UserName = "TestUser";

        [Fact]
        public async Task CreateHeroShouldCreateHeroInDatabase()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroCreateInputModel inputModel = new HeroCreateInputModel() { Fraction = Fraction.Sheep, Gender = Gender.Male };
            HeroService heroService = this.GetHeroServiceInitialValues(context);

            // Act
            await heroService.CreateHero(inputModel);

            // Assert
            Assert.True(context.Heroes.Any());
        }

        [Fact]
        public async Task CreateHeroShouldCreateHeroWithProperName()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroCreateInputModel inputModel = new HeroCreateInputModel() { Fraction = Fraction.Sheep, Gender = Gender.Male };
            HeroService heroService = this.GetHeroServiceInitialValues(context);

            // Act
            await heroService.CreateHero(inputModel);
            Hero hero = context.Heroes.First();

            // Assert
            Assert.Equal(UserName, hero.Name);
        }

        [Fact]
        public async Task CreateHeroShouldIgnoreNameInInputModel()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroCreateInputModel inputModel = new HeroCreateInputModel() { Fraction = Fraction.Sheep, Gender = Gender.Male, Name = "Name" };
            HeroService heroService = this.GetHeroServiceInitialValues(context);

            // Act
            await heroService.CreateHero(inputModel);
            Hero hero = context.Heroes.First();

            // Assert
            Assert.Equal(UserName, hero.Name);
        }

        [Fact]
        public async Task CreateHeroShouldUseGenderAndFractionFromInputModel()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            Fraction fraction = Fraction.Pig;
            Gender gender = Gender.Female;
            HeroCreateInputModel inputModel = new HeroCreateInputModel() { Fraction = fraction, Gender = gender };
            HeroService heroService = this.GetHeroServiceInitialValues(context);

            // Act
            await heroService.CreateHero(inputModel);

            // Assert
            Assert.True(context.Heroes.Any(x => x.Fraction == fraction && x.Gender == gender));
        }

        [Fact]
        public async Task GetCurrentHeroShouldReturnCorrectHero()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroService heroService = this.GetHeroServiceInitialValues(context);

            // Act
            Hero hero = await heroService.GetCurrentHero();

            // Assert
            Assert.Equal(UserName, hero.Name);
        }

        [Fact]
        public async Task GetHeroByIdShouldReturnCorrectHero()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroService heroService = this.GetHeroServiceInitialValues(context);
            Hero heroOne = new Hero() { Name = "HeroOne" };
            Hero heroTwo = new Hero() { Name = "HeroTwo" };
            await context.Heroes.AddAsync(heroOne);
            await context.Heroes.AddAsync(heroTwo);

            // Act
            Hero heroById = await heroService.GetHeroById(heroTwo.Id);

            // Assert
            Assert.Equal(heroTwo, heroById);
        }

        [Fact]
        public async Task GetHeroByIdInvalidIdShouldReturnNull()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroService heroService = this.GetHeroServiceInitialValues(context);
            Hero hero = new Hero() { Name = "HeroOne" };
            await context.Heroes.AddAsync(hero);

            // Act
            Hero heroById = await heroService.GetHeroById(hero.Id + 1);

            // Assert
            Assert.Null(heroById);
        }

        [Fact]
        public async Task GetHeroByNameInvalidNameShouldReturnNull()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroService heroService = this.GetHeroServiceInitialValues(context);
            Hero hero = new Hero() { Name = "HeroOne" };
            await context.Heroes.AddAsync(hero);

            // Act
            Hero heroByName = await heroService.GetHeroByName("Invalid name");

            // Assert
            Assert.Null(heroByName);
        }

        [Fact]
        public async Task ValidateCurrentHeroLocationShouldNotThrowExceptionIfWorkStatusIdle()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroService heroService = this.GetHeroServiceInitialValues(context);

            // Act
            await heroService.ValidateCurrentHeroLocation(WorkStatus.Farm);
        }

        [Fact]
        public async Task ValidateCurrentHeroLocationShouldNotThrowExceptionIfWorkStatusSame()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroService heroService = this.GetHeroService(context);

            // Act
            await heroService.ValidateCurrentHeroLocation(WorkStatus.Farm);
        }

        [Fact]
        public async Task ValidateCurrentHeroLocationShouldThrowExceptionIfWorkStatusDifferent()
        {
            // Arrange
            FarmHeroesDbContext context = FarmHeroesDbContextInMemoryInitializer.InitializeContext();
            HeroService heroService = this.GetHeroService(context);

            // Act
            await Assert.ThrowsAsync<FarmHeroesException>(async () => await heroService.ValidateCurrentHeroLocation(WorkStatus.Mine));
        }

        private HeroService GetHeroServiceInitialValues(FarmHeroesDbContext context)
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

            HeroService heroService = new HeroService(context, mapper, userServiceMock.Object);

            return heroService;
        }

        private HeroService GetHeroService(FarmHeroesDbContext context)
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

            HeroService heroService = new HeroService(context, mapper, userServiceMock.Object);

            return heroService;
        }
    }
}
