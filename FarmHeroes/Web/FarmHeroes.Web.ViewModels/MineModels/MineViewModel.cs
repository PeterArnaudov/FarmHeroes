namespace FarmHeroes.Web.ViewModels.MineModels
{
    using System;

    public class MineViewModel
    {
        public bool DoesWork { get; set; }

        public bool IsWorkFinished { get; set; }

        public DateTime? WorkUntil { get; set; }
    }
}
