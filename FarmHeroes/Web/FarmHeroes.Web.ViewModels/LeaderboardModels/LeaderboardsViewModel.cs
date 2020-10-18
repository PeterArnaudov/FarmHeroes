namespace FarmHeroes.Web.ViewModels.LeaderboardModels
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class LeaderboardsViewModel
    {
        public int PlayersPerPage => 25;

        public List<PlayerLeaderboardsViewModel> Players { get; set; }

        public string Criteria { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int CurrentPageMinimumPosition => ((this.CurrentPage - 1) * this.PlayersPerPage) + 1;

        public int CurrentPageMaximumPosition => this.CurrentPage * this.PlayersPerPage;
    }
}
