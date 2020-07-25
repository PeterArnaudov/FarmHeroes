namespace FarmHeroes.Web.ViewModels.MineModels
{
    using FarmHeroes.Data.Models.Enums;
    using System;

    public class MineViewModel
    {
        public bool IsWorkFinished => this.ChronometerWorkUntil < DateTime.UtcNow && this.WorkStatus == WorkStatus.Mine;

        public bool DoesWork => this.WorkStatus == WorkStatus.Mine;

        public DateTime? ChronometerWorkUntil { get; set; }

        public WorkStatus WorkStatus { get; set; }
    }
}
