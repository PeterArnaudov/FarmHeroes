namespace FarmHeroes.Data.Models.HeroModels
{
    using System;

    public class AmuletBag
    {
        public int Id { get; set; }

        public DateTime ActiveUntil { get; set; }

        public int OnIdleAmuletId { get; set; }

        public int OnPlayerAttackAmuletId { get; set; }

        public int OnMonsterAttackAmuletId { get; set; }

        public int OnFarmAmuletId { get; set; }

        public int OnMineAmuletId { get; set; }

        public int OnPatrolAmuletId { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
