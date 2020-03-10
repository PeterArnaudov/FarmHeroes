namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.LevelModels;

    public class LevelService : ILevelService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IHeroService heroService;

        public LevelService(FarmHeroesDbContext context, IHeroService heroService)
        {
            this.context = context;
            this.heroService = heroService;
        }

        public async Task<int> GetCurrentHeroLevel()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            return hero.Level.CurrentLevel;
        }

        public async Task GiveCurrentHeroExperience(int experience)
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.Level.CurrentLevel == 100)
            {
                return;
            }

            hero.Level.CurrentExperience += experience;

            if (hero.Level.CurrentExperience >= hero.Level.NeededExperience)
            {
                await this.LevelUpCurrentHero();
            }

            await this.context.SaveChangesAsync();
        }

        public async Task GiveHeroExperienceById(int id, int experience)
        {
            Hero hero = await this.heroService.GetHeroById(id);

            if (hero.Level.CurrentLevel == 100)
            {
                return;
            }

            hero.Level.CurrentExperience += experience;

            if (hero.Level.CurrentExperience >= hero.Level.NeededExperience)
            {
                await this.LevelUpHeroById(id);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task LevelUpCurrentHero()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.Level.CurrentLevel == 100)
            {
                return;
            }

            int experienceLeft = hero.Level.CurrentExperience - hero.Level.NeededExperience;

            hero.Level.CurrentExperience = experienceLeft;
            hero.Level.NeededExperience = LevelFormulas.CalculateNextExperienceNeeded(hero.Level.NeededExperience);
            hero.Level.CurrentLevel++;

            await this.context.SaveChangesAsync();
        }

        public async Task LevelUpHeroById(int id)
        {
            Hero hero = await this.heroService.GetHeroById(id);

            if (hero.Level.CurrentLevel == 100)
            {
                return;
            }

            int experienceLeft = hero.Level.CurrentExperience - hero.Level.NeededExperience;

            hero.Level.CurrentExperience = experienceLeft;
            hero.Level.NeededExperience = LevelFormulas.CalculateNextExperienceNeeded(hero.Level.NeededExperience);
            hero.Level.CurrentLevel++;

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateLevel(LevelModifyInputModel inputModel)
        {
            Hero hero = await this.heroService.GetHeroByName(inputModel.Name);

            hero.Level.CurrentLevel = inputModel.LevelCurrentLevel;
            hero.Level.CurrentExperience = inputModel.LevelCurrentExperience;
            hero.Level.NeededExperience = inputModel.LevelNeededExperience;

            await this.context.SaveChangesAsync();
        }
    }
}
