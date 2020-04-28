namespace FarmHeroes.Web.ViewModels.FarmModels
{
    using System;

    public class FarmViewModel
    {
        public long Salary { get; set; }

        public bool IsWorkFinished { get; set; }

        public bool DoesWork { get; set; }

        public DateTime? ChronometerWorkUntil { get; set; }
    }
}
