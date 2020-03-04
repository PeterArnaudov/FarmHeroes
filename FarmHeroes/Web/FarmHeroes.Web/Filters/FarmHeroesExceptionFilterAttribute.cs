namespace FarmHeroes.Web.Filters
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Web.ApiControllers;
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
            string[] apiControllerNames = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(ApiController).IsAssignableFrom(type))
                .Select(type => type.Name.Replace("Controller", string.Empty))
                .ToArray();

            if (context.Exception is FarmHeroesException)
            {
                FarmHeroesException exception = context.Exception as FarmHeroesException;
                context.ExceptionHandled = true;

                if (apiControllerNames.Contains(context.RouteData.Values["Controller"]))
                {
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = new JsonResult(new { exception.Message, exception.Instructions });
                }
                else
                {
                    this.tempDataDictionaryFactory
                        .GetTempData(context.HttpContext)
                        .Add("ExceptionMessage", exception.Message);

                    this.tempDataDictionaryFactory
                        .GetTempData(context.HttpContext)
                        .Add("ExceptionInstructions", exception.Instructions);

                    context.Result = new RedirectResult(exception.RedirectPath);
                }
            }
        }
    }
}
