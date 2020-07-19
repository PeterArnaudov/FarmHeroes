namespace FarmHeroes.Web.ViewModels.DungeonModels
{
    using FarmHeroes.Data.Models.Enums;
    using System;

    public class DungeonIndexViewModel
    {
        public int ResourcePouchDungeonKeys { get; set; }

        public DateTime? ChronometerCannotDungeonUntil { get; set; }

        public bool CanEnterDungeon => this.ChronometerCannotDungeonUntil < DateTime.UtcNow;

        public WorkStatus WorkStatus { get; set; }
    }
}
