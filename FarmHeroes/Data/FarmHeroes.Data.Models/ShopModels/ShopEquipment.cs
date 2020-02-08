namespace FarmHeroes.Data.Models.ShopModels
{
    using System;

    public abstract class ShopEquipment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte RequiredLevel { get; set; }

        public int Bonus { get; set; }

        public string ImageUrl { get; set; }
    }
}
