namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.News;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.NewsModels;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class NewsService : INewsService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public NewsService(FarmHeroesDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<T[]> GetAllNews<T>()
        {
            News[] news = await this.context.News.ToArrayAsync();

            return this.mapper.Map<T[]>(news);
        }

        public async Task<T> GetNewsModelById<T>(int id)
        {
            News news = await this.context.News.FindAsync(id);

            return this.mapper.Map<T>(news);
        }

        public async Task AddNews(NewsInputModel inputModel)
        {
            News news = this.mapper.Map<News>(inputModel);

            await this.context.News.AddAsync(news);
            await this.context.SaveChangesAsync();
        }

        public async Task EditNews(NewsInputModel inputModel)
        {
            News news = this.mapper.Map<News>(inputModel);

            this.context.News.Update(news);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteNews(int id)
        {
            News news = await this.context.News.FindAsync(id);

            this.context.News.Remove(news);
            await this.context.SaveChangesAsync();
        }
    }
}
