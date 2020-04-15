namespace FarmHeroes.Web
{
    using FarmHeroes.Web.BackgroundTasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                .ConfigureServices(services =>
                    {
                        services.AddHostedService<PassiveIncomeTask>();
                        services.AddHostedService<NotificationDeleteTask>();
                        services.AddHostedService<DailyLimitsResetTask>();
                    });
    }
}
