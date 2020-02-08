namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class Health
    {
        private const int InitialHealth = 50;

        public int Id { get; set; }

        public long Current { get; set; } = InitialHealth;

        public long Maximum { get; set; } = InitialHealth;

        public virtual Hero Hero { get; set; }
    }
}
