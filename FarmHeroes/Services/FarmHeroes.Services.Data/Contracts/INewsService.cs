namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Web.ViewModels.NewsModels;
    using System;
    using System.Threading.Tasks;

    public interface INewsService
    {
        Task<T[]> GetAllNews<T>();

        Task<T> GetNewsModelById<T>(int id);

        Task AddNews(NewsInputModel inputModel);

        Task EditNews(NewsInputModel inputModel);

        Task DeleteNews(int id);
    }
}
