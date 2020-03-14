namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.DashboardModels;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class DashboardService : IDashboardService
    {
        private readonly FarmHeroesDbContext context;

        public DashboardService(FarmHeroesDbContext context)
        {
            this.context = context;
        }

        public SimpleReportViewModel[] GetUserRegistrationsReport()
        {
            SimpleReportViewModel[] report = this.context.Users
                .GroupBy(x => new { x.CreatedOn.Month, x.CreatedOn.Year })
                .Select(x => new SimpleReportViewModel()
            {
                DimensionOne = $"{x.Key.Month:d2}.{x.Key.Year}",
                Value = x.Count(),
            })
                .ToArray();

            return report;
        }

        public SimpleReportViewModel[] GetFractionDistributionReport()
        {
            SimpleReportViewModel[] report = this.context.Heroes
                .GroupBy(x => x.Fraction)
                .Select(x => new SimpleReportViewModel()
                {
                    DimensionOne = x.Key.ToString(),
                    Value = x.Count(),
                })
                .ToArray();

            return report;
        }
    }
}
