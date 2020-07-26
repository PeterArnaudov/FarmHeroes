namespace FarmHeroes.Web.ViewModels.NewsModels
{
    using System;

    public class NewsListViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
    }
}
