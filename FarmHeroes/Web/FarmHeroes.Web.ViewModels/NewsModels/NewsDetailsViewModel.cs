namespace FarmHeroes.Web.ViewModels.NewsModels
{
    using System;

    public class NewsDetailsViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
    }
}
