namespace FarmHeroes.Web.Areas.Administration.Controllers
{
    using FarmHeroes.Data.Models;
    using FarmHeroes.Services.Data;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Web.ViewModels.Administration.Dashboard;
    using FarmHeroes.Web.ViewModels.DashboardModels;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    public class DashboardController : AdministrationController
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult UserStatistics()
        {
            return this.View();
        }

        public IActionResult HeroStatistics()
        {
            return this.View();
        }

        public IActionResult UserRegistrations()
        {
            SimpleReportViewModel[] viewModel = this.dashboardService.GetUserRegistrationsReport();

            return this.View(viewModel);
        }

        public IActionResult FractionDistribution()
        {
            SimpleReportViewModel[] viewModel = this.dashboardService.GetFractionDistributionReport();

            return this.View(viewModel);
        }
    }
}
