﻿namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using System;
    using System.Threading.Tasks;

    public interface IEquipmentService
    {
        Task<EquippedSet> GetCurrentHeroEquipedSet();

        Task<EquippedSet> GetEquippedSetById(int id);

        Task Equip(int id);

        Task<AmuletViewModel> EquipAmulet(int id);
    }
}
