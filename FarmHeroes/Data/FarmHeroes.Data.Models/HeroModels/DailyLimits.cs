namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class DailyLimits
    {
        public int Id { get; set; }

        public int PatrolsDone { get; set; }

        public int PatrolLimit { get; set; } = 3;

        public virtual Hero Hero { get; set; }
    }
}
