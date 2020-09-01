namespace FarmHeroes.Web.ViewModels.SmithModels
{
    using System;

    public class SmithAmuletViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double InitialBonus { get; set; }

        public int InitialPrice { get; set; }

        public double Bonus { get; set; }

        public int Level { get; set; }

        public string ImageUrl { get; set; }

        public int UpgradeCost { get; set; }
    }
}
