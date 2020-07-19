namespace FarmHeroes.Web.ViewModels.DungeonModels
{
    using System;

    public class DungeonWalkingViewModel
    {
        public int DungeonInformationCurrentFloor { get; set; }

        public int DungeonInformationMonstersDefeatedOnCurrentFloor { get; set; }

        public int MonstersAvailable => 4 - this.DungeonInformationMonstersDefeatedOnCurrentFloor;

        public DateTime? ChronometerWorkUntil { get; set; }

        public bool IsWalking => this.ChronometerWorkUntil < DateTime.UtcNow;

        public bool AreMonsterAvailable => this.MonstersAvailable > 0;
    }
}
