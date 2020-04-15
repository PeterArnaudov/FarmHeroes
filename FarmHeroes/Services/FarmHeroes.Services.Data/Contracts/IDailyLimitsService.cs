namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.HeroModels;
    using System;
    using System.Threading.Tasks;

    public interface IDailyLimitsService
    {
        Task UpdateDailyLimits(DailyLimits dailyLimits);

        Task ResetDailyLimits();
    }
}
