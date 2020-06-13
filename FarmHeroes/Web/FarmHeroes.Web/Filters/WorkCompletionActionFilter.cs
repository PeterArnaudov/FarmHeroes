namespace FarmHeroes.Web.Filters
{
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Threading.Tasks;

    public class WorkCompletionActionFilter : IAsyncActionFilter
    {
        private readonly IHeroService heroService;
        private readonly IBattlefieldService battlefieldService;
        private readonly IFarmService farmService;

        public WorkCompletionActionFilter(IHeroService heroService, IBattlefieldService battlefieldService, IFarmService farmService)
        {
            this.heroService = heroService;
            this.battlefieldService = battlefieldService;
            this.farmService = farmService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Hero hero = await this.heroService.GetHero();

            if (hero.WorkStatus == WorkStatus.Battlefield && this.CheckIfWorkIsFinished(hero))
            {
                await this.battlefieldService.Collect();
            }
            else if (hero.WorkStatus == WorkStatus.Farm && this.CheckIfWorkIsFinished(hero))
            {
                await this.farmService.Collect();
            }

            await next();
        }

        private bool CheckIfWorkIsFinished(Hero hero)
        {
            return hero.Chronometer.WorkUntil <= DateTime.UtcNow;
        }
    }
}
