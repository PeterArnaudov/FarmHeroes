namespace FarmHeroes.Data.Models.News
{
    using System;

    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
    }
}
