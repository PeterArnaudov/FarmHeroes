namespace FarmHeroes.Web.Filters
{
    using FarmHeroes.Data.Models;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Mvc;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data;

    public class LevelRequirementAttribute : ActionFilterAttribute
    {
        private readonly int requiredLevel;

        public LevelRequirementAttribute(int requiredLevel)
        {
            this.requiredLevel = requiredLevel;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var levelService = context.HttpContext.RequestServices.GetService<ILevelService>();
                var localizationService = context.HttpContext.RequestServices.GetService<LocalizationService>();

                var heroLevel = levelService.GetCurrentHeroLevel().Result;

                if (heroLevel < this.requiredLevel)
                {
                    throw new FarmHeroesException(
                        localizationService.ExceptionLocalizer("Level-Requirement-Message"),
                        string.Format(localizationService.ExceptionLocalizer("Level-Requirement-Instruction"), this.requiredLevel),
                        Redirects.HeroRedirect);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
