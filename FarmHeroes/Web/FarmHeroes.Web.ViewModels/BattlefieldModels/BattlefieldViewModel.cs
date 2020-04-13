namespace FarmHeroes.Web.ViewModels.BattlefieldModels
{
    using System;

    public class BattlefieldViewModel
    {
        public bool CanAttackHero { get; set; }

        public DateTime? CannotAttackHeroUntil { get; set; }

        public bool CanAttackMonster { get; set; }

        public DateTime? CannotAttackMonsterUntil { get; set; }

        public int PatrolsDone { get; set; }

        public bool IsOnPatrol { get; set; }

        public bool CanGoOnPatrol { get; set; }

        public bool IsPatrolFinished { get; set; }

        public DateTime? PatrolUntil { get; set; }
    }
}
