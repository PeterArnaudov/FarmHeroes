namespace FarmHeroes.Web.ViewModels.AmuletBagModels
{
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using System;

    public class AmuletBagViewModel
    {
        public DateTime ActiveUntil { get; set; }

        public bool IsActivePermanently => this.ActiveUntil == DateTime.MaxValue;

        public bool IsActive => this.ActiveUntil > DateTime.UtcNow;

        public int OnIdleAmuletId { get; set; }

        public int OnPlayerAttackAmuletId { get; set; }

        public int OnMonsterAttackAmuletId { get; set; }

        public int OnFarmAmuletId { get; set; }

        public int OnMineAmuletId { get; set; }

        public int OnPatrolAmuletId { get; set; }

        public AmuletSelectViewModel[] Amulets { get; set; }
    }
}
