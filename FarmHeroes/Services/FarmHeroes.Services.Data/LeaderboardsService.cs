namespace FarmHeroes.Services.Data
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.LeaderboardModels;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LeaderboardsService : ILeaderboardsService
    {
        private const int PlayersPerPage = 25;

        private readonly FarmHeroesDbContext context;

        public LeaderboardsService(FarmHeroesDbContext context)
        {
            this.context = context;
        }

        public async Task<LeaderboardsViewModel> GetLeaderboardData(LeaderboardsInputModel inputModel)
        {
            List<PlayerLeaderboardsViewModel> players = await this.context.Heroes
                .Select(x => new PlayerLeaderboardsViewModel
                {
                    Username = x.Name,
                    Points = (int)typeof(Statistics).GetProperty(inputModel.Criteria).GetValue(x.Statistics),
                })
                .ToListAsync();

            int pagesCount = (players.Count / PlayersPerPage) + 1;

            players = players
                .OrderByDescending(x => x.Points)
                .Skip((PlayersPerPage * inputModel.Page) - PlayersPerPage)
                .Take(PlayersPerPage).ToList();

            return new LeaderboardsViewModel()
            {
                Players = players,
                Criteria = inputModel.Criteria,
                Pages = pagesCount,
                CurrentPage = inputModel.Page,
            };
        }
    }
}
