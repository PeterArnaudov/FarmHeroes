namespace FarmHeroes.Web.ViewModels.DungeonModels
{
    using System;

    public class DungeonWalkingViewModel
    {
        public int DungeonInformationCurrentFloor { get; set; }

        public int DungeonInformationMonstersDefeatedOnCurrentFloor { get; set; }

        public DateTime? ChronometerWorkUntil { get; set; }
    }
}
