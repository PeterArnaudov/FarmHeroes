namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class DungeonInformation
    {
        public int Id { get; set; }

        public int CurrentFloor { get; set; }

        public int MonstersDefeatedOnCurrentFloor { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
