namespace FarmHeroes.Data.Models.ShopModels
{
    using System;

    using FarmHeroes.Data.Models.Enums;

    public class ShopBonus
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CrystalsPrice { get; set; }

        public double InitialBonus { get; set; }

        public BonusType Type { get; set; }

        public int Days { get; set; }

        public bool IsPermanent { get; set; }

        public string AttainableFrom { get; set; }
    }
}
