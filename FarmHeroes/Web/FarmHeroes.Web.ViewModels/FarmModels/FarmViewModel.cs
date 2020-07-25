namespace FarmHeroes.Web.ViewModels.FarmModels
{
    using FarmHeroes.Data.Models.Enums;
    using System;

    public class FarmViewModel
    {
        public long Salary { get; set; }

        public bool IsWorkFinished => this.ChronometerWorkUntil < DateTime.UtcNow && this.WorkStatus == WorkStatus.Farm;

        public bool DoesWork => this.WorkStatus == WorkStatus.Farm;

        public DateTime? ChronometerWorkUntil { get; set; }

        public WorkStatus WorkStatus { get; set; }
    }
}
