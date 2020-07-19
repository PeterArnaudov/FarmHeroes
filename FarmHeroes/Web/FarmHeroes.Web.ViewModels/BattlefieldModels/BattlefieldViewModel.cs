namespace FarmHeroes.Web.ViewModels.BattlefieldModels
{
    using FarmHeroes.Data.Models.Enums;
    using System;

    public class BattlefieldViewModel
    {
        public DateTime? ChronometerCannotAttackHeroUntil { get; set; }

        public bool CanAttackHero =>
            this.ChronometerCannotAttackHeroUntil < DateTime.UtcNow
            || this.ChronometerCannotAttackHeroUntil == null;

        public DateTime? ChronometerCannotAttackMonsterUntil { get; set; }

        public bool CanAttackMonster =>
            this.ChronometerCannotAttackMonsterUntil < DateTime.UtcNow
            || this.ChronometerCannotAttackMonsterUntil == null;

        public int DailyLimitsPatrolsDone { get; set; }

        public int DailyLimitsPatrolResets { get; set; }

        public WorkStatus WorkStatus { get; set; }

        public bool IsOnPatrol => this.WorkStatus == WorkStatus.Battlefield;

        public bool CanGoOnPatrol => this.WorkStatus == WorkStatus.Idle;

        public DateTime? ChronometerWorkUntil { get; set; }

        public bool IsPatrolFinished => this.ChronometerWorkUntil < DateTime.UtcNow;
    }
}
