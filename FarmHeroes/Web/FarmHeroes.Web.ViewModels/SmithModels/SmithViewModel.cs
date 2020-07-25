namespace FarmHeroes.Web.ViewModels.SmithModels
{
    using System;
    using System.Linq;

    public class SmithViewModel
    {
        public SmithEquipmentViewModel[] Items { get; set; }

        public SmithEquipmentViewModel[] UpgradeableItems => this.Items.Where(x => x.Level < 25).ToArray();

        public SmithAmuletViewModel[] Amulets { get; set; }

        public SmithAmuletViewModel[] UpgradeableAmulets => this.Amulets.Where(x => x.Level < 100).ToArray();
    }
}
