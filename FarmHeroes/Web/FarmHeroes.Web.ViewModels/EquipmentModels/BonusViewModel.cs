namespace FarmHeroes.Web.ViewModels.EquipmentModels
{
    using System;

    public class BonusViewModel
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public DateTime ActiveUntil { get; set; }
    }
}
