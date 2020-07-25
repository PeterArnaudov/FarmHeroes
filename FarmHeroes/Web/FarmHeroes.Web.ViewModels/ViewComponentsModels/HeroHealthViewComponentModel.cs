namespace FarmHeroes.Web.ViewModels.ViewComponentsModels
{
    using System;

    public class HeroHealthViewComponentModel
    {
        public long Current { get; set; }

        public long Maximum { get; set; }

        public double Percent => Math.Round((double)(100 * this.Current / this.Maximum));
    }
}
