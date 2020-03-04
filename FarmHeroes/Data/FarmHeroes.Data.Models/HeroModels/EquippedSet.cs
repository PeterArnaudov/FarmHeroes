namespace FarmHeroes.Data.Models.HeroModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EquippedSet
    {
        public int Id { get; set; }

        public virtual List<HeroEquipment> Equipped { get; set; } = new List<HeroEquipment>();

        public int? AmuletId { get; set; }

        public virtual HeroAmulet Amulet { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
