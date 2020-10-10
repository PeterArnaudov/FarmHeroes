namespace FarmHeroes.Web.Filters
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Web.ApiControllers;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.AspNetCore.Http.Extensions;
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
                context.ExceptionHandled = true;

                if (exception.IsAjax)
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

                    if (exception.RedirectPath != string.Empty)
                    {
                        context.Result = new RedirectResult(exception.RedirectPath);
                    }
                    else
                    {
                        context.Result = new RedirectResult(context.HttpContext.Request.Headers["Referer"].ToString());
                    }
                }
            }
        }
    }
}
