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
        private readonly ILogger<PassiveIncomeTask> _logger;
        private readonly IServiceScopeFactory scopeFactory;
        private Timer _timer;

        public PassiveIncomeTask(ILogger<PassiveIncomeTask> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            this._logger.LogInformation("Passive Income Task running.");

            this._timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3600));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            this._logger.LogInformation("Passive Income Task is stopping.");

            this._timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this._timer?.Dispose();
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref this.executionCount);

            using (var scope = this.scopeFactory.CreateScope())
            {
                var resourcePouchService = scope.ServiceProvider.GetRequiredService<IResourcePouchService>();
                resourcePouchService.GivePassiveIncome().Wait();
            }

            this._logger.LogInformation(
                "Passive Income Task is working. Count: {Count}", count);
        }
    }
}
