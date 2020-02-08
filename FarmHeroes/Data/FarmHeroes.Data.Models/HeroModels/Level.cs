namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class Level
    {
        private const int LevelTwoNeededExperience = 20;

        public int Id { get; set; }

        public int CurrentLevel { get; set; } = 1;

        public int CurrentExperience { get; set; }

        public int NeededExperience { get; set; } = LevelTwoNeededExperience;

        public virtual Hero Hero { get; set; }
    }
}
