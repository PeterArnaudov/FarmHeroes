namespace FarmHeroes.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;
    using Microsoft.EntityFrameworkCore;

    public class ResourcePouchService : IResourcePouchService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IHeroService heroService;
        private readonly IMapper mapper;

        public ResourcePouchService(FarmHeroesDbContext context, IHeroService heroService, IMapper mapper)
        {
            this.context = context;
            this.heroService = heroService;
            this.mapper = mapper;
        }

        public async Task<ResourcePouch> GetResourcePouch(int id = 0)
        {
            Hero hero = await this.heroService.GetHero(id);

            return hero.ResourcePouch;
        }

        public async Task<TViewModel> GetCurrentHeroResourcesViewModel<TViewModel>()
        {
            ResourcePouch resourcePouch = await this.GetResourcePouch();
            TViewModel viewModel = this.mapper.Map<TViewModel>(resourcePouch);

            return viewModel;
        }

        public async Task IncreaseGold(int gold, int id = 0)
        {
            ResourcePouch resources = await this.GetResourcePouch(id);
            resources.Gold += gold;

            await this.context.SaveChangesAsync();
        }

        public async Task DecreaseGold(int gold, int id = 0)
        {
            ResourcePouch resources = await this.GetResourcePouch(id);

            this.CheckIfHeroHasEnoughGold(resources, gold);

            resources.Gold -= gold;

            await this.context.SaveChangesAsync();
        }

        public async Task IncreaseCrystals(int crystals, int id = 0)
        {
            ResourcePouch resources = await this.GetResourcePouch(id);
            resources.Crystals += crystals;

            await this.context.SaveChangesAsync();
        }

        public async Task DecreaseCrystals(int crystals, int id = 0)
        {
            ResourcePouch resources = await this.GetResourcePouch(id);

            this.CheckIfHeroHasEnoughCrystals(resources, crystals);

            resources.Crystals -= crystals;

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateResourcePouch(ResourcePouchModifyInputModel inputModel)
        {
            Hero hero = await this.heroService.GetHeroByName(inputModel.Name);

            hero.ResourcePouch.Gold = inputModel.ResourcePouchGold;
            hero.ResourcePouch.Crystals = inputModel.ResourcePouchCrystals;

            await this.context.SaveChangesAsync();
        }

        public async Task GivePassiveIncome()
        {
            List<Hero> heroes = await this.context.Heroes.ToListAsync();
            heroes.ForEach(x => x.ResourcePouch.Gold += ResourceFormulas.CalculatePassiveIncome(x.Level.CurrentLevel));
            await this.context.SaveChangesAsync();
        }

        private void CheckIfHeroHasEnoughGold(ResourcePouch resources, int goldNeeded)
        {
            if (resources.Gold < goldNeeded)
            {
                throw new FarmHeroesException(
                    ResourcePouchExceptionMessages.NotEnoughGoldMessage,
                    ResourcePouchExceptionMessages.NotEnoughGoldInstrctuon,
                    Redirects.BattlefieldRedirect);
            }
        }

        private void CheckIfHeroHasEnoughCrystals(ResourcePouch resources, int crystalsNeeded)
        {
            if (resources.Crystals < crystalsNeeded)
            {
                throw new FarmHeroesException(
                    ResourcePouchExceptionMessages.NotEnoughCrystalsMessage,
                    ResourcePouchExceptionMessages.NotEnoughCrystalsInstruction,
                    Redirects.MineRedirect);
            }
        }
    }
}
