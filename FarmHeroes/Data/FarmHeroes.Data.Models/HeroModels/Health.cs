namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class Health
    {
        private const int InitialHealth = 50;

        public int Id { get; set; }

        public int Current { get; set; } = InitialHealth;

        public int Maximum { get; set; } = InitialHealth;

        public virtual Hero Hero { get; set; }
    }
}
