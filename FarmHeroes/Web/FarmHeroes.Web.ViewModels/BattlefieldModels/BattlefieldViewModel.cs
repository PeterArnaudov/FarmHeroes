namespace FarmHeroes.Web.ViewModels.BattlefieldModels
{
    using System;

    public class BattlefieldViewModel
    {
        public bool CanAttackHero { get; set; }

        public DateTime? ChronometerCannotAttackHeroUntil { get; set; }

        public bool CanAttackMonster { get; set; }

        public DateTime? ChronometerCannotAttackMonsterUntil { get; set; }

        public int DailyLimitsPatrolsDone { get; set; }

        public bool IsOnPatrol { get; set; }

        public bool CanGoOnPatrol { get; set; }

        public bool IsPatrolFinished { get; set; }

        public DateTime? ChronometerWorkUntil { get; set; }
    }
}
