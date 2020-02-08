namespace FarmHeroes.Data.Models.MappingModels
{
    using FarmHeroes.Data.Models.FightModels;
    using FarmHeroes.Data.Models.HeroModels;

    public class HeroFight
    {
        public int HeroId { get; set; }

        public virtual Hero Hero { get; set; }

        public int FightId { get; set; }

        public virtual Fight Fight { get; set; }
    }
}
