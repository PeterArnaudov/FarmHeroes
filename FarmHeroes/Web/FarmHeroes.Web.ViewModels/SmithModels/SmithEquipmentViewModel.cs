namespace FarmHeroes.Web.ViewModels.SmithModels
{
    using System;

    public class SmithEquipmentViewModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Mastery { get; set; }

        public int UpgradeCost { get; set; }
    }
}
