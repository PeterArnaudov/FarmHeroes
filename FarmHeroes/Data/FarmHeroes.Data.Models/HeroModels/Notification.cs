namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    using FarmHeroes.Data.Common.Models;
    using FarmHeroes.Data.Models.Enums;

    public class Notification : BaseModel<int>
    {
        public NotificationType Type { get; set; }

        public int HeroId { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
