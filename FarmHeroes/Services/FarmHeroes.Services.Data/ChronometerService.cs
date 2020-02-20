namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;

    public class ChronometerService : IChronometerService
    {
        private readonly FarmHeroesDbContext context;
        private readonly IHeroService heroService;
        private readonly IMapper mapper;

        public ChronometerService(FarmHeroesDbContext context, IHeroService heroService, IMapper mapper)
        {
            this.context = context;
            this.heroService = heroService;
            this.mapper = mapper;
        }

        public async Task<Chronometer> GetCurrentHeroChronometer()
        {
            Hero hero = await this.heroService.GetCurrentHero();
            Chronometer chronometer = hero.Chronometer;

            return chronometer;
        }

        public async Task<TViewModel> GetCurrentHeroChronometerViewModel<TViewModel>()
        {
            Chronometer chronometer = await this.GetCurrentHeroChronometer();
            TViewModel viewModel = this.mapper.Map<TViewModel>(chronometer);

            return viewModel;
        }

        public async Task<Chronometer> GetChronometerById(int id)
        {
            Chronometer chronometer = await this.context.Chronometers.FindAsync(id);

            return chronometer;
        }

        public async Task SetWorkUntil(int minutes, WorkStatus workStatus)
        {
            Hero hero = await this.heroService.GetCurrentHero();

            if (hero.Chronometer.WorkUntil != null)
            {
                throw new FarmHeroesException(
                    "You are already working somewhere.",
                    "Cancel or finish your current work.",
                    "/Farm");
            }

            hero.Chronometer.WorkUntil = DateTime.UtcNow.AddMinutes(minutes);
            hero.WorkStatus = workStatus;

            await this.context.SaveChangesAsync();
        }

        public async Task NullifyWorkUntil()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            hero.Chronometer.WorkUntil = null;
            hero.WorkStatus = WorkStatus.Idle;

            await this.context.SaveChangesAsync();
        }

        public async Task SetCannotAttackHeroUntilById(int id, int minutes)
        {
            Chronometer chronometer = await this.GetChronometerById(id);
            chronometer.CannotAttackHeroUntil = DateTime.UtcNow.AddMinutes(minutes);

            await this.context.SaveChangesAsync();
        }

        public async Task SetCannotAttackMonsterUntilById(int id, int minutes)
        {
            Chronometer chronometer = await this.GetChronometerById(id);
            chronometer.CannotAttackMonsterUntil = DateTime.UtcNow.AddMinutes(minutes);

            await this.context.SaveChangesAsync();
        }

        public async Task SetCannotBeAttackedUntilById(int id, int minutes)
        {
            Chronometer chronometer = await this.GetChronometerById(id);
            chronometer.CannotBeAttackedUntil = DateTime.UtcNow.AddMinutes(minutes);

            await this.context.SaveChangesAsync();
        }
    }
}
