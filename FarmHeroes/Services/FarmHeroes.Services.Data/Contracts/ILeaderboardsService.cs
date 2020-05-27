namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Web.ViewModels.LeaderboardModels;
    using System;
    using System.Threading.Tasks;

    public interface ILeaderboardsService
    {
        Task<LeaderboardsViewModel> GetLeaderboardData(LeaderboardsInputModel inputModel);
    }
}
