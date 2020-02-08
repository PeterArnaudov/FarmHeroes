namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public abstract class HeroEquipment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte RequiredLevel { get; set; }

        public int Bonus { get; set; }

        public int Level { get; set; }

        public string ImageUrl { get; set; }

        public int InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual EquippedSet EquippedSet { get; set; }
    }
}
