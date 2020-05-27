namespace FarmHeroes.Web.Controllers
{
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.LeaderboardModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public class LeaderboardsController : BaseController
    {
        private readonly ILeaderboardsService leaderboardsService;

        public LeaderboardsController(ILeaderboardsService leaderboardsService)
        {
            this.leaderboardsService = leaderboardsService;
        }

        public async Task<IActionResult> Index(LeaderboardsInputModel inputModel)
        {
            LeaderboardsViewModel viewModel = await this.leaderboardsService.GetLeaderboardData(inputModel);

            return this.View(viewModel);
        }
    }
}
