namespace FarmHeroes.Web.ApiControllers
{
    using System.Threading.Tasks;

    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Formulas;
    using FarmHeroes.Web.ViewModels.ResourcePouchModels;

    using Microsoft.AspNetCore.Mvc;

    public class WorkController : ApiController
    {
        private readonly ILevelService levelService;
        private readonly IFarmService farmService;
        private readonly IMineService mineService;
        private readonly IBattlefieldService battlefieldService;
        private readonly IChronometerService chronometerService;

        public WorkController(ILevelService levelService, IFarmService farmService, IMineService mineService, IBattlefieldService battlefieldService, IChronometerService chronometerService)
        {
            this.levelService = levelService;
            this.farmService = farmService;
            this.mineService = mineService;
            this.battlefieldService = battlefieldService;
            this.chronometerService = chronometerService;
        }

        [HttpGet("StartWork/{location}")]
        public async Task<ActionResult<object>> StartWork(string location)
        {
                int hours = 0;
                int minutes = 0;
                int seconds = 0;

                if (location == "farm")
                {
                    hours = await this.farmService.StartWork();
                }
                else if (location == "mine")
                {
                    minutes = await this.mineService.InitiateDig();
                }
                else if (location == "battlefield")
                {
                    minutes = await this.battlefieldService.StartPatrol();
                }

                object result = new
                {
                    Hours = hours,
                    Minutes = minutes,
                    Seconds = seconds,
                };

            return result;
        }

        [HttpGet("Collect/{location}")]
        public async Task<ActionResult<CollectedResourcesViewModel>> Collect(string location)
        {
                CollectedResourcesViewModel collectedResources = new CollectedResourcesViewModel();

                if (location == "farm")
                {
                    collectedResources = await this.farmService.Collect();
                }
                else if (location == "mine")
                {
                    collectedResources = await this.mineService.Collect();
                }
                else if (location == "battlefield")
                {
                    collectedResources = await this.battlefieldService.Collect();
                }

                return collectedResources;
        }

        [HttpGet("CancelWork")]
        public async Task<ActionResult<object>> CancelWork()
        {
                await this.chronometerService.NullifyWorkUntil();

                object result = new
                {
                    FarmSalary = FarmFormulas.CalculateFarmSalaryPerHour(this.levelService.GetCurrentHeroLevel().Result),
                };

                return result;
        }
    }
}
