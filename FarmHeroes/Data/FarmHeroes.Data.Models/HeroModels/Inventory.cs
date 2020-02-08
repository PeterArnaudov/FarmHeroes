namespace FarmHeroes.Data.Models.HeroModels
{
    using System;
    using System.Collections.Generic;

    using FarmHeroes.Data.Models.HeroModels;

    public class Inventory
    {
        private const int InitialItemsCap = 5;

        public int Id { get; set; }

        public int ItemsCap { get; set; } = InitialItemsCap;

        public virtual List<HeroEquipment> Storage { get; set; } = new List<HeroEquipment>();

        public virtual Hero Hero { get; set; }
    }
}
