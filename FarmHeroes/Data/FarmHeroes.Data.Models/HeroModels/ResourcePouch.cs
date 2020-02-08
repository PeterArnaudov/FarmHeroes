namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class ResourcePouch
    {
        public int Id { get; set; }

        public int Gold { get; set; }

        public int Crystals { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
