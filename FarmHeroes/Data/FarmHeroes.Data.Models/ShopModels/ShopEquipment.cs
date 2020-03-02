namespace FarmHeroes.Data.Models.ShopModels
{
    using System;

    public abstract class ShopEquipment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int RequiredLevel { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Mastery { get; set; }

        public string ImageUrl { get; set; }
    }
}
