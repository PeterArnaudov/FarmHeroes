namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class ResourcePouch
    {
        private const int InitialGold = 500;

        public int Id { get; set; }

        public int Gold { get; set; } = InitialGold;

        public int Crystals { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
