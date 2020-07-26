namespace FarmHeroes.Web.Areas.Administration.Controllers.Dashboard
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.NewsModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class NewsController : AdministrationController
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

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsInputModel inputModel)
        {
            await this.newsService.AddNews(inputModel);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            NewsInputModel inputModel = await this.newsService.GetNewsModelById<NewsInputModel>(id);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewsInputModel inputModel)
        {
            await this.newsService.EditNews(inputModel);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.newsService.DeleteNews(id);

            return this.RedirectToAction("Index");
        }
    }
}
