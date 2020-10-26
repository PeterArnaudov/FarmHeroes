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
        private readonly IDungeonService dungeonService;
        private readonly IHarbourService harbourService;

        public WorkCompletionActionFilter(IHeroService heroService, IBattlefieldService battlefieldService, IFarmService farmService, IDungeonService dungeonService, IHarbourService harbourService)
        {
            this.heroService = heroService;
            this.battlefieldService = battlefieldService;
            this.farmService = farmService;
            this.dungeonService = dungeonService;
            this.harbourService = harbourService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                try
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
                    else if (hero.WorkStatus == WorkStatus.Dungeon && this.CheckIfWorkIsFinished(hero))
                    {
                        await this.dungeonService.AttackMonster();
                    }

                    if (this.CheckIfSailingIsFinished(hero))
                    {
                        await this.harbourService.Collect();
                    }
                    else
                    {
                        await this.harbourService.ManagerSetSail(hero.Id);
                    }
                }
                catch
                {
                    await next();
                    return;
                }
            }

            await next();
        }

        private bool CheckIfWorkIsFinished(Hero hero)
        {
            return hero.Chronometer.WorkUntil <= DateTime.UtcNow;
        }

        private bool CheckIfSailingIsFinished(Hero hero)
        {
            return hero.Chronometer.SailingUntil.HasValue && hero.Chronometer.SailingUntil < DateTime.UtcNow;
        }
    }
}
