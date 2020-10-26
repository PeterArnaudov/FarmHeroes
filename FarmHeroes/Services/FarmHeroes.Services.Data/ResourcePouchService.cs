namespace FarmHeroes.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
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
            if (id == 0)
            {
                id = (await this.heroService.GetHero()).ResourcePouchId;
            }

            ResourcePouch resourcePouch = await this.context.ResourcePouches.FindAsync(id);

            return resourcePouch;
        }

        public async Task UpdateResourcePouch(ResourcePouchModifyInputModel inputModel)
        {
            Hero hero = await this.heroService.GetHeroByName(inputModel.Name);

            hero.ResourcePouch.Gold = inputModel.ResourcePouchGold;
            hero.ResourcePouch.Crystals = inputModel.ResourcePouchCrystals;

            await this.context.SaveChangesAsync();
        }

        public async Task IncreaseResource(string resource, int amount, int id = 0)
        {
            ResourcePouch resources = await this.GetResourcePouch(id);

            int currentAmount = (int)typeof(ResourcePouch).GetProperty(resource).GetValue(resources);
            typeof(ResourcePouch).GetProperty(resource).SetValue(resources, currentAmount + amount);

            await this.context.SaveChangesAsync();
        }

        public async Task<TViewModel> GetCurrentHeroResourcesViewModel<TViewModel>()
        {
            ResourcePouch resourcePouch = await this.GetResourcePouch();
            TViewModel viewModel = this.mapper.Map<TViewModel>(resourcePouch);

            return viewModel;
        }

        public async Task<bool> DecreaseResource(string resourceName, int amount, int id = 0)
        {
            ResourcePouch resources = await this.GetResourcePouch(id);

            this.CheckIfHeroHasEnoughResource(resources, resourceName, amount);

            int currentAmount = (int)typeof(ResourcePouch).GetProperty(resourceName).GetValue(resources);
            typeof(ResourcePouch).GetProperty(resourceName).SetValue(resources, currentAmount - amount);

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetResource(string resourceName, int id = 0)
        {
            ResourcePouch resources = await this.GetResourcePouch(id);

            int currentAmount = (int)typeof(ResourcePouch).GetProperty(resourceName).GetValue(resources);

            return currentAmount;
        }

        public async Task GivePassiveIncome()
        {
            List<Hero> heroes = await this.context.Heroes.ToListAsync();
            heroes.ForEach(x => x.ResourcePouch.Gold += ResourceFormulas.CalculatePassiveIncome(x.Level.CurrentLevel));
            await this.context.SaveChangesAsync();
        }

        private void CheckIfHeroHasEnoughResource(ResourcePouch resources, string resourceName, int amount)
        {
            int currentAmount = (int)typeof(ResourcePouch).GetProperty(resourceName).GetValue(resources);

            if (currentAmount < amount)
            {
                throw new FarmHeroesException(
                    string.Format(ResourcePouchExceptionMessages.NotEnoughResourceMessage, this.SeparateWords(resourceName)),
                    string.Format(ResourcePouchExceptionMessages.NotEnoughResourceInstruction, this.SeparateWords(resourceName)),
                    string.Empty,
                    true);
            }
        }

        private string SeparateWords(string resourceName)
        {
            return Regex.Replace(resourceName, @"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])", " ").ToLower();
        }
    }
}
