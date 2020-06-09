namespace FarmHeroes.Services.Data
{
    using AutoMapper;
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Constants;
    using FarmHeroes.Services.Data.Constants.ExceptionMessages;
    using FarmHeroes.Services.Data.Contracts;
    using FarmHeroes.Services.Data.Exceptions;
    using FarmHeroes.Web.ViewModels.AmuletBagModels;
    using FarmHeroes.Web.ViewModels.EquipmentModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AmuletBagService : IAmuletBagService
    {
        private const int ExtendRentPrice = 140;
        private const int ExtendRentDays = 14;
        private const int PurchaseAmuletBagPrice = 3500;

        private readonly IHeroService heroService;
        private readonly IEquipmentService equipmentService;
        private readonly IResourcePouchService resourcePouchService;
        private readonly FarmHeroesDbContext context;
        private readonly IMapper mapper;

        public AmuletBagService(IHeroService heroService, IEquipmentService equipmentService, IResourcePouchService resourcePouchService, FarmHeroesDbContext context, IMapper mapper)
        {
            this.heroService = heroService;
            this.equipmentService = equipmentService;
            this.resourcePouchService = resourcePouchService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AmuletBagViewModel> GetAmuletBagViewModel()
        {
            Hero hero = await this.heroService.GetHero();

            AmuletBagViewModel viewModel = this.mapper.Map<AmuletBagViewModel>(hero.AmuletBag);
            viewModel.Amulets = this.mapper.Map<AmuletSelectViewModel[]>(hero.Inventory.Amulets);

            return viewModel;
        }

        public async Task ExtendRent()
        {
            Hero hero = await this.heroService.GetHero();

            this.CheckIfAmuletBagIsPurchased(hero.AmuletBag);

            hero.AmuletBag.ActiveUntil = hero.AmuletBag.ActiveUntil < DateTime.UtcNow
                ? DateTime.UtcNow.AddDays(ExtendRentDays)
                : hero.AmuletBag.ActiveUntil.AddDays(ExtendRentDays);

            await this.resourcePouchService.DecreaseCrystals(ExtendRentPrice);

            await this.context.SaveChangesAsync();
        }

        public async Task Purchase()
        {
            Hero hero = await this.heroService.GetHero();

            this.CheckIfAmuletBagIsPurchased(hero.AmuletBag);

            hero.AmuletBag.ActiveUntil = DateTime.MaxValue;

            await this.resourcePouchService.DecreaseCrystals(PurchaseAmuletBagPrice);

            await this.context.SaveChangesAsync();
        }

        public async Task Set(AmuletBagViewModel inputModel)
        {
            Hero hero = await this.heroService.GetHero();

            int[] idsToSet = typeof(AmuletBagViewModel)
                .GetProperties()
                .Where(x => x.Name.StartsWith("On"))
                .Select(x => (int)x.GetValue(inputModel))
                .ToArray();

            if (idsToSet.Except(hero.Inventory.Amulets.Select(x => x.Id)).Any())
            {
                throw new FarmHeroesException(
                    AmuletBagExceptionMessages.HeroDoesNotOwnAmuletsMessage,
                    AmuletBagExceptionMessages.HeroDoesNotOwnAmuletsInstruction,
                    Redirects.AmuletBagRedirect);
            }

            hero.AmuletBag.OnIdleAmuletId = inputModel.OnIdleAmuletId;
            hero.AmuletBag.OnPlayerAttackAmuletId = inputModel.OnPlayerAttackAmuletId;
            hero.AmuletBag.OnMonsterAttackAmuletId = inputModel.OnMonsterAttackAmuletId;
            hero.AmuletBag.OnFarmAmuletId = inputModel.OnFarmAmuletId;
            hero.AmuletBag.OnMineAmuletId = inputModel.OnMineAmuletId;
            hero.AmuletBag.OnPatrolAmuletId = inputModel.OnPatrolAmuletId;

            await this.context.SaveChangesAsync();
        }

        public async Task EquipAmulet(string action)
        {
            Hero hero = await this.heroService.GetHero();
            int amuletId = (int)typeof(AmuletBag).GetProperty("On" + action + "AmuletId").GetValue(hero.AmuletBag);

            if (amuletId == 0)
            {
                return;
            }

            await this.equipmentService.EquipAmulet(amuletId);
        }

        private void CheckIfAmuletBagIsPurchased(AmuletBag amuletBag)
        {
            if (amuletBag.ActiveUntil == DateTime.MaxValue)
            {
                throw new FarmHeroesException(
                    AmuletBagExceptionMessages.AmuletBagAlreadyPurchasedMessage,
                    AmuletBagExceptionMessages.AmuletBagAlreadyPurchasedInstruction,
                    Redirects.AmuletBagRedirect);
            }
        }
    }
}
