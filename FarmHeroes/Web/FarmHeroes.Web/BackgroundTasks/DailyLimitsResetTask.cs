using FarmHeroes.Data;
using FarmHeroes.Data.Models.HeroModels;
using FarmHeroes.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FarmHeroes.Web.BackgroundTasks
{
    public class DailyLimitsResetTask : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<PassiveIncomeTask> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private Timer timer;

        public DailyLimitsResetTask(ILogger<PassiveIncomeTask> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Daily Limit Reset Task running.");

            this.timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(86400));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Daily Limit Reset Task is stopping.");

            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref this.executionCount);

            using (var scope = this.scopeFactory.CreateScope())
            {
                var dailyLimitsService = scope.ServiceProvider.GetRequiredService<IDailyLimitsService>();
                dailyLimitsService.ResetDailyLimits().Wait();
            }

            this.logger.LogInformation(
                "Daily Limit Reset Task is working. Count: {Count}", count);
        }
    }
}
