namespace FarmHeroes.Data.Models.HeroModels
{
    using FarmHeroes.Data.Models.Enums;

    public class HeroEquipment
    {
        public int Id { get; set; }

        public EquipmentType Type { get; set; }

        public string Name { get; set; }

        public int RequiredLevel { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Mastery { get; set; }

        public int Level { get; set; } = 1;

        public string ImageUrl { get; set; }

        public int InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }

        public int? EquippedSetId { get; set; }

        public virtual EquippedSet EquippedSet { get; set; }
    }
}
