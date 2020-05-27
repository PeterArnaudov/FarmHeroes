namespace FarmHeroes.Web.ViewModels.LeaderboardModels
{
    using System;
    using System.Collections.Generic;

    public class LeaderboardsViewModel
    {
        public List<PlayerLeaderboardsViewModel> Players { get; set; }

        public string Criteria { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; } = 1;
    }
}
