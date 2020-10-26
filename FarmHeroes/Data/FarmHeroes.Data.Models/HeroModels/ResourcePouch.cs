namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class ResourcePouch
    {
        private const int InitialGold = 500;

        public int Id { get; set; }

        public int Gold { get; set; } = InitialGold;

        public int Crystals { get; set; }

        public int Fish { get; set; }

        public int DungeonKeys { get; set; }

        public int Boats { get; set; }

        public int Ships { get; set; }

        public int Submarines { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
