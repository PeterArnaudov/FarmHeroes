namespace FarmHeroes.Web.ViewModels.NotificationModels
{
    using System;

    public class NotificationViewModel
    {
        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Content { get; set; }

        public int? Gold { get; set; }

        public int? Experience { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool IsNew { get; set; } = true;
    }
}
