namespace FarmHeroes.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Mapping;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class HeroService : IHeroService
    {
        private const string MaleSheepAvatarUrl = "/images/avatars/sheep-male.jpg";
        private const string FemaleSheepAvatarUrl = "/images/avatars/sheep-female.jpg";
        private const string MalePigAvatarUrl = "/images/avatars/pig-male.jpg";
        private const string FemalePigAvatarUrl = "/images/avatars/pig-female.jpg";

        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public HeroService(FarmHeroesDbContext context, IMapper mapper, IUserService userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
        }

        public async Task CreateHero(HeroCreateInputModel inputModel)
        {
            Hero hero = this.mapper.Map<Hero>(inputModel);
            hero.User = await this.userService.GetApplicationUser();
            hero.Name = hero.User.UserName;

            if (hero.Fraction == Fraction.Sheep && hero.Gender == Gender.Male)
            {
                hero.AvatarUrl = MaleSheepAvatarUrl;
            }
            else if (hero.Fraction == Fraction.Sheep && hero.Gender == Gender.Female)
            {
                hero.AvatarUrl = FemaleSheepAvatarUrl;
            }
            else if (hero.Fraction == Fraction.Pig && hero.Gender == Gender.Male)
            {
                hero.AvatarUrl = MalePigAvatarUrl;
            }
            else if (hero.Fraction == Fraction.Pig && hero.Gender == Gender.Female)
            {
                hero.AvatarUrl = FemalePigAvatarUrl;
            }

            await this.context.Heroes.AddAsync(hero);
            await this.context.SaveChangesAsync();
        }

        public async Task<Hero> GetCurrentHero()
        {
            ApplicationUser user = await this.userService.GetApplicationUser();

            return user.Hero;
        }

        public async Task<Hero> GetHeroById(int id)
        {
            Hero hero = await this.context.Heroes.FindAsync(id);
            return hero;
        }

        public async Task<Hero> GetHeroByName(string name)
        {
            Hero hero = await this.context.Heroes.SingleOrDefaultAsync(h => h.Name == name);
            return hero;
        }

        public async Task<TViewModel> GetCurrentHeroViewModel<TViewModel>()
        {
            Hero hero = await this.GetCurrentHero();

            return this.mapper.Map<TViewModel>(hero);
        }

        public async Task<TViewModel> GetHeroViewModelById<TViewModel>(int id)
        {
            Hero hero = await this.context.Heroes.SingleOrDefaultAsync(x => x.Id == id);

            return this.mapper.Map<TViewModel>(hero);
        }

        public async Task<TViewModel> GetHeroViewModelByName<TViewModel>(string name)
        {
            Hero hero = await this.context.Heroes.SingleOrDefaultAsync(x => x.Name == name);

            return this.mapper.Map<TViewModel>(hero);
        }

        public async Task<bool> ValidateCurrentHeroLocation(WorkStatus workStatus)
        {
            Hero hero = await this.GetCurrentHero();
            bool validLocation = hero.WorkStatus == WorkStatus.Idle || workStatus == hero.WorkStatus;

            return validLocation;
        }

        public async Task UpdateBasicInfo(HeroModifyBasicInfoInputModel inputModel)
        {
            Hero hero = await this.GetHeroByName(inputModel.Name);

            hero.Fraction = inputModel.Fraction;
            hero.Gender = inputModel.Gender;

            if (hero.Fraction == Fraction.Sheep && hero.Gender == Gender.Male)
            {
                hero.AvatarUrl = MaleSheepAvatarUrl;
            }
            else if (hero.Fraction == Fraction.Sheep && hero.Gender == Gender.Female)
            {
                hero.AvatarUrl = FemaleSheepAvatarUrl;
            }
            else if (hero.Fraction == Fraction.Pig && hero.Gender == Gender.Male)
            {
                hero.AvatarUrl = MalePigAvatarUrl;
            }
            else if (hero.Fraction == Fraction.Pig && hero.Gender == Gender.Female)
            {
                hero.AvatarUrl = FemalePigAvatarUrl;
            }

            await this.context.SaveChangesAsync();
        }
    }
}
