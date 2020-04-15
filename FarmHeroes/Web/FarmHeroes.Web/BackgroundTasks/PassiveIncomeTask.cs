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
    public class PassiveIncomeTask : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<PassiveIncomeTask> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private Timer timer;

        public PassiveIncomeTask(ILogger<PassiveIncomeTask> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Passive Income Task running.");

            this.timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3600));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Passive Income Task is stopping.");

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
                var resourcePouchService = scope.ServiceProvider.GetRequiredService<IResourcePouchService>();
                resourcePouchService.GivePassiveIncome().Wait();
            }

            this.logger.LogInformation(
                "Passive Income Task is working. Count: {Count}", count);
        }
    }
}
