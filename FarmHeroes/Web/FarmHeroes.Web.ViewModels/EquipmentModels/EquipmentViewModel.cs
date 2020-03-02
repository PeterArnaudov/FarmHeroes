namespace FarmHeroes.Web.ViewModels.EquipmentModels
{
    using System;

    public class EquipmentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int RequiredLevel { get; set; }

        public int Level { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Mastery { get; set; }

        public string ImageUrl { get; set; }
    }
}
