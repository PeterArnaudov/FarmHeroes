namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class Chronometer
    {
        public int Id { get; set; }

        public DateTime? WorkUntil { get; set; }

        public DateTime? CannotAttackHeroUntil { get; set; } = DateTime.UtcNow;

        public DateTime? CannotAttackMonsterUntil { get; set; } = DateTime.UtcNow;

        public DateTime? CannotBeAttackedUntil { get; set; } = DateTime.UtcNow;

        public DateTime? CannotDungeonUntil { get; set; } = DateTime.UtcNow;

        public virtual Hero Hero { get; set; }
    }
}
