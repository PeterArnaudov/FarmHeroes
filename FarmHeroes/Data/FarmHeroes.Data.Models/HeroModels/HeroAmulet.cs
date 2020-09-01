namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class HeroAmulet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double InitialBonus { get; set; }

        public int InitialPrice { get; set; }

        public double Bonus { get; set; }

        public int Level { get; set; } = 1;

        public string ImageUrl { get; set; }

        public int InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual EquippedSet EquippedSet { get; set; }
    }
}
