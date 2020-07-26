namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.NewsModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class NewsController : BaseController
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            NewsListViewModel[] viewModel = await this.newsService.GetAllNews<NewsListViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            NewsDetailsViewModel viewModel = await this.newsService.GetNewsModelById<NewsDetailsViewModel>(id);

            return this.View(viewModel);
        }
    }
}
