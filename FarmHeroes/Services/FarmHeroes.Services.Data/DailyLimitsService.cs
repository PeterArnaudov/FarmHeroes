namespace FarmHeroes.Services.Data
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
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

        public async Task ResetDailyLimits()
        {
            List<DailyLimits> dailyLimits = await this.context.DailyLimits.ToListAsync();
            dailyLimits.ForEach(x =>
            {
                x.PatrolsDone = 0;
                x.PatrolResets = 0;
            });

            await this.context.SaveChangesAsync();
        }
    }
}
