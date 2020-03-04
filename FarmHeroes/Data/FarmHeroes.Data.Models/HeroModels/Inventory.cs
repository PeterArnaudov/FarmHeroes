namespace FarmHeroes.Data.Models.HeroModels
{
    using System;
    using System.Collections.Generic;

    using FarmHeroes.Data.Models.HeroModels;

    public class Inventory
    {
        private const int InitialItemsCapacity = 5;

        public int Id { get; set; }

        public int MaximumCapacity { get; set; } = InitialItemsCapacity;

        public virtual List<HeroEquipment> Items { get; set; } = new List<HeroEquipment>();

        public virtual List<HeroAmulet> Amulets { get; set; } = new List<HeroAmulet>();

        public virtual Hero Hero { get; set; }
    }
}
