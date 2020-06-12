namespace FarmHeroes.Data.Models.ShopModels
{
    using FarmHeroes.Data.Models.Enums;
    using System;

    public class ShopEquipment
    {
        public int Id { get; set; }

        public EquipmentType Type { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int RequiredLevel { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Mastery { get; set; }

        public int Dexterity { get; set; }

        public string ImageUrl { get; set; }
    }
}
