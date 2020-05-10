namespace FarmHeroes.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.Enums;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Web.ViewModels.ChronometerModels;

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

        public async Task SetWorkUntil(int seconds, WorkStatus workStatus)
        {
            Hero hero = await this.heroService.GetCurrentHero();

            this.CheckIfCurrentlyWorking(hero);

            hero.Chronometer.WorkUntil = DateTime.UtcNow.AddSeconds(seconds);
            hero.WorkStatus = workStatus;

            await this.context.SaveChangesAsync();
        }

        public async Task NullifyWorkUntil()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            this.CheckIfPatrolling(hero);

            hero.Chronometer.WorkUntil = null;
            hero.WorkStatus = WorkStatus.Idle;

            await this.context.SaveChangesAsync();
        }

        public async Task SetCannotAttackHeroUntilById(int id, int seconds)
        {
            Chronometer chronometer = await this.GetChronometerById(id);
            chronometer.CannotAttackHeroUntil = DateTime.UtcNow.AddSeconds(seconds);

            await this.context.SaveChangesAsync();
        }

        public async Task SetCannotAttackMonsterUntilById(int id, int seconds)
        {
            Chronometer chronometer = await this.GetChronometerById(id);
            chronometer.CannotAttackMonsterUntil = DateTime.UtcNow.AddSeconds(seconds);

            await this.context.SaveChangesAsync();
        }

        public async Task SetCannotBeAttackedUntilById(int id, int seconds)
        {
            Chronometer chronometer = await this.GetChronometerById(id);
            chronometer.CannotBeAttackedUntil = DateTime.UtcNow.AddSeconds(seconds);

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateChronometer(ChronometerModifyInputModel inputModel)
        {
            Hero hero = await this.heroService.GetHeroByName(inputModel.Name);

            hero.Chronometer.WorkUntil = inputModel.ChronometerWorkUntil;
            hero.Chronometer.CannotAttackHeroUntil = inputModel.ChronometerCannotAttackHeroUntil;
            hero.Chronometer.CannotBeAttackedUntil = inputModel.ChronometerCannotBeAttackedUntil;
            hero.Chronometer.CannotAttackMonsterUntil = inputModel.ChronometerCannotAttackMonsterUntil;

            if (hero.Chronometer.WorkUntil == null)
            {
                hero.WorkStatus = WorkStatus.Idle;
            }

            await this.context.SaveChangesAsync();
        }

        private void CheckIfCurrentlyWorking(Hero hero)
        {
            if (hero.Chronometer.WorkUntil != null)
            {
                throw new FarmHeroesException(
                    ChronometerExceptionMessages.CurrentlyWorkingMessage,
                    ChronometerExceptionMessages.CurrentlyWorkingInstruction,
                    Redirects.FarmRedirect);
            }
        }

        private void CheckIfPatrolling(Hero hero)
        {
            if (hero.WorkStatus == WorkStatus.Battlefield && hero.Chronometer.WorkUntil > DateTime.UtcNow)
            {
                throw new FarmHeroesException(
                    ChronometerExceptionMessages.PatrolCancelMessage,
                    ChronometerExceptionMessages.PatrolCancelInstruction,
                    Redirects.BattlefieldRedirect);
            }
        }
    }
}
