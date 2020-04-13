namespace FarmHeroes.Services.Data
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using System;
    using System.Threading.Tasks;

    public class DailyLimitsService : IDailyLimitsService
    {
        private readonly FarmHeroesDbContext context;

        public DailyLimitsService(FarmHeroesDbContext context)
        {
            this.context = context;
        }

        public async Task UpdateDailyLimits(DailyLimits dailyLimits)
        {
            this.context.DailyLimits.Update(dailyLimits);
            await this.context.SaveChangesAsync();
        }
    }
}
