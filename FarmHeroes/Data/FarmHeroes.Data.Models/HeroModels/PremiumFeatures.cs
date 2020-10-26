namespace FarmHeroes.Data.Models.HeroModels
{
    public class PremiumFeatures
    {
        public int Id { get; set; }

        public bool HarbourManager { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
