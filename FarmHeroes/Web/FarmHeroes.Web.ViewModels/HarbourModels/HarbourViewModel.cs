namespace FarmHeroes.Web.ViewModels.HarbourModels
{
    using System;

    public class HarbourViewModel
    {
        public int ResourcePouchBoats { get; set; }

        public int ResourcePouchShips { get; set; }

        public int ResourcePouchSubmarines { get; set; }

        public DateTime? ChronometerSailingUntil { get; set; }

        public bool PremiumFeaturesHarbourManager { get; set; }

        public bool IsSailing => this.ChronometerSailingUntil > DateTime.UtcNow;
    }
}
