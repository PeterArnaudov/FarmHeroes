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
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IChronometerService chronometerService;

        public HeroService(FarmHeroesDbContext context, IMapper mapper, IUserService userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
        }

        public async Task CreateHero(HeroCreateInputModel inputModel)
        {
            Hero hero = this.mapper.Map<Hero>(inputModel);
            hero.User = await this.userService.GetApplicationUserAsync();
            hero.Name = this.userService.GetUsername();

            await this.context.Heroes.AddAsync(hero);
            await this.context.SaveChangesAsync();
        }

        public async Task<Hero> GetCurrentHero()
        {
            ApplicationUser user = await this.userService.GetApplicationUserAsync();
            Hero hero = await this.context.Heroes.SingleOrDefaultAsync(x => x.User == user);

            return hero;
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

        public async Task<bool> ValidateCurrentHeroLocation(WorkStatus workStatus)
        {
            Hero hero = await this.GetCurrentHero();
            bool validLocation = hero.WorkStatus == WorkStatus.Idle || workStatus == hero.WorkStatus;

            return validLocation;
        }
    }
}
