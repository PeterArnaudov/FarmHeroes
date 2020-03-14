namespace FarmHeroes.Web
{
    using System.Reflection;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Common;
    using FarmHeroes.Data.Common.Repositories;
    using FarmHeroes.Data.Models;
    using FarmHeroes.Data.Repositories;
    using FarmHeroes.Data.Seeding;
    using FarmHeroes.Services.Data;
    using FarmHeroes.Services.Data.Contracts;
    using Farmheroes.Services.Data.Utilities;
    using FarmHeroes.Services.Mapping;
    using FarmHeroes.Services.Messaging;
    using FarmHeroes.Web.ViewModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using FarmHeroes.Web.Filters;
    using Microsoft.AspNetCore.Mvc;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FarmHeroesDbContext>(
                options => options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<FarmHeroesDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(AutoValidateAntiforgeryTokenAttribute));
                options.Filters.Add(typeof(FarmHeroesExceptionFilterAttribute));
            });
            services.AddRazorPages();

            services.AddSingleton(this.configuration);
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(FarmHeroesProfile));

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IHeroService, HeroService>();
            services.AddTransient<IMineService, MineService>();
            services.AddTransient<IFarmService, FarmService>();
            services.AddTransient<IResourcePouchService, ResourcePouchService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<ICharacteristicsService, CharacteristicsService>();
            services.AddTransient<IHealthService, HealthService>();
            services.AddTransient<ILevelService, LevelService>();
            services.AddTransient<IChronometerService, ChronometerService>();
            services.AddTransient<IBattlefieldService, BattlefieldService>();
            services.AddTransient<IFightService, FightService>();
            services.AddTransient<IMonsterService, MonsterService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IShopService, ShopService>();
            services.AddTransient<IEquipmentService, EquipmentService>();
            services.AddTransient<ISmithService, SmithService>();
            services.AddTransient<IDashboardService, DashboardService>();

            // Filters
            services.AddTransient<FarmHeroesExceptionFilterAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<FarmHeroesDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
