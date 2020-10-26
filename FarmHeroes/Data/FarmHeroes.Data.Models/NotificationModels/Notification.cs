namespace FarmHeroes.Data.Models.NotificationModels.HeroModels
{
    using System;

    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;

    public class Notification
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Content { get; set; }

        public int? Gold { get; set; }

        public int? Crystals { get; set; }

        public int? Fish { get; set; }

        public int? Experience { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool IsNew { get; set; } = true;

        public NotificationType Type { get; set; }

        public int HeroId { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
