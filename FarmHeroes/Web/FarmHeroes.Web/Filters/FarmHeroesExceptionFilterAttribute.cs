namespace FarmHeroes.Web.Filters
{
    using System;

    using FarmHeroes.Services.Data.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class FarmHeroesExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;

        public FarmHeroesExceptionFilterAttribute(ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is FarmHeroesException)
            {
                FarmHeroesException exception = context.Exception as FarmHeroesException;

                this.tempDataDictionaryFactory
                    .GetTempData(context.HttpContext)
                    .Add("ExceptionMessage", exception.Message);

                this.tempDataDictionaryFactory
                    .GetTempData(context.HttpContext)
                    .Add("ExceptionInstructions", exception.Instructions);

                context.ExceptionHandled = true;

                context.Result = new RedirectResult(exception.RedirectPath);
            }
        }
    }
}
