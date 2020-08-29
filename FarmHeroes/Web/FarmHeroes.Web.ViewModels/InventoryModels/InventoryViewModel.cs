namespace FarmHeroes.Web.ViewModels.InventoryModels
{
    using FarmHeroes.Data.Models.HeroModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class InventoryViewModel
    {
        public int MaximumCapacity { get; set; }

        public int UpgradeCost { get; set; }

        public bool MaximumCapacityReached => this.MaximumCapacity >= 20;

        public List<HeroEquipment> Items { get; set; }

        public bool HasItems => this.Items.Count > 0;

        public List<HeroAmulet> Amulets { get; set; }

        public bool HasAmulets => this.Amulets.Count > 0;

        public List<HeroBonus> Bonuses { get; set; }

        public List<HeroBonus> ActiveBonuses => this.Bonuses.Where(x => x.ActiveUntil > DateTime.UtcNow).ToList();

        public bool HasBonuses => this.ActiveBonuses.Count > 0;
    }
}
