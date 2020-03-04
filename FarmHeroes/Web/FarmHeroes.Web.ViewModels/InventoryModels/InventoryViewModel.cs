namespace FarmHeroes.Web.ViewModels.InventoryModels
{
    using FarmHeroes.Data.Models.HeroModels;
    using System;
    using System.Collections.Generic;

    public class InventoryViewModel
    {
        public int MaximumCapacity { get; set; }

        public int UpgradeCost { get; set; }

        public List<HeroEquipment> Items { get; set; }

        public List<HeroAmulet> Amulets { get; set; }
    }
}
