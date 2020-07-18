namespace FarmHeroes.Web.ViewModels.ViewComponentsModels
{
    using System;
    using System.Linq;
    using FarmHeroes.Web.ViewModels.EquipmentModels;

    public class SideMenuBonusesViewComponentModel
    {
        public BonusViewModel[] Bonuses { get; set; }

        public BonusViewModel[] ActiveBonuses => this.Bonuses
            .Where(x => x.ActiveUntil != DateTime.MaxValue && x.ActiveUntil > DateTime.UtcNow)
            .ToArray();
    }
}
