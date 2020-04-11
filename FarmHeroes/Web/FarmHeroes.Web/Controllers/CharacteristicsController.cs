﻿namespace FarmHeroes.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.Filters;
    using FarmHeroes.Web.ViewModels.CharacteristcsModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CharacteristicsController : BaseController
    {
        private readonly ICharacteristicsService characteristicsService;

        public CharacteristicsController(ICharacteristicsService characteristicsService)
        {
            this.characteristicsService = characteristicsService;
        }

        public async Task<IActionResult> Index()
        {
            CharacteristicsPracticeViewModel viewModel =
                await this.characteristicsService.GetCurrentHeroCharacteristicsViewModelAsync<CharacteristicsPracticeViewModel>();

            return this.View(viewModel);
        }
    }
}
