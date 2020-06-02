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
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Mapping;
    using FarmHeroes.Web.ViewModels.HeroModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class HeroService : IHeroService
    {
        private const string AvatarUrlFormat = "/images/avatars/{0}-{1}.jpg";

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

            hero.AvatarUrl = string.Format(
                AvatarUrlFormat,
                hero.Fraction.ToString().ToLower(),
                hero.Gender.ToString().ToLower());

            await this.context.Heroes.AddAsync(hero);
            await this.context.SaveChangesAsync();
        }

        public async Task<Hero> GetHero(int id = 0)
        {
            Hero hero = id == 0 ? this.userService.GetApplicationUser().Result.Hero : await this.context.Heroes.FindAsync(id);

            return hero;
        }

        public async Task<Hero> GetHeroByName(string name)
        {
            Hero hero = await this.context.Heroes.SingleOrDefaultAsync(h => h.Name == name);
            return hero;
        }

        public async Task<TViewModel> GetCurrentHeroViewModel<TViewModel>()
        {
            Hero hero = await this.GetHero();

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

        public async Task ValidateCurrentHeroLocation(WorkStatus workStatus)
        {
            Hero hero = await this.GetHero();
            bool validLocation = hero.WorkStatus == WorkStatus.Idle || workStatus == hero.WorkStatus;

            if (!validLocation)
            {
                throw new FarmHeroesException(
                    HeroExceptionMessages.InvalidLocationMessage,
                    string.Format(HeroExceptionMessages.InvalidLocationInstruction, hero.WorkStatus, workStatus),
                    Redirects.HeroRedirect);
            }
        }

        public async Task UpdateBasicInfo(HeroModifyBasicInfoInputModel inputModel)
        {
            Hero hero = await this.GetHeroByName(inputModel.Name);

            hero.Fraction = inputModel.Fraction;
            hero.Gender = inputModel.Gender;

            hero.AvatarUrl = string.Format(
                AvatarUrlFormat,
                hero.Fraction.ToString().ToLower(),
                hero.Gender.ToString().ToLower());

            await this.context.SaveChangesAsync();
        }
    }
}
