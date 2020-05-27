namespace FarmHeroes.Web.ViewModels.LeaderboardModels
{
    using System;

    public class LeaderboardsInputModel
    {
        public string Criteria { get; set; } = "Wins";

        public int Page { get; set; } = 1;
    }
}
