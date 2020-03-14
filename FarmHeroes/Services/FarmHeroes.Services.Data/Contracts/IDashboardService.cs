namespace FarmHeroes.Services.Data.Contracts
{
    using FarmHeroes.Web.ViewModels.DashboardModels;

    public interface IDashboardService
    {
        SimpleReportViewModel[] GetUserRegistrationsReport();

        SimpleReportViewModel[] GetFractionDistributionReport();
    }
}
