﻿using FarmHeroes.Data;
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
    public class HarbourTask : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<HarbourTask> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private Timer timer;

        public HarbourTask(ILogger<HarbourTask> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Harbour Task running.");

            this.timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(300));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Harbour Task is stopping.");

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
                var harbourService = scope.ServiceProvider.GetRequiredService<IHarbourService>();
                harbourService.CollectAll().Wait();
            }

            this.logger.LogInformation(
                "Harbour Task is working. Count: {Count}", count);
        }
    }
}
