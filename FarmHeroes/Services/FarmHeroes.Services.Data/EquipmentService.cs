namespace FarmHeroes.Services.Data
{
    using FarmHeroes.Data;
    using FarmHeroes.Data.Models.HeroModels;
    using FarmHeroes.Services.Data.Contracts;
    using System;
    using System.Threading.Tasks;

    public class EquipmentService : IEquipmentService
    {
        private readonly IHeroService heroService;
        private readonly FarmHeroesDbContext context;

        public EquipmentService(IHeroService heroService, FarmHeroesDbContext context)
        {
            this.heroService = heroService;
            this.context = context;
        }

        public async Task<EquippedSet> GetCurrentHeroEquipedSet()
        {
            Hero hero = await this.heroService.GetCurrentHero();

            return hero.EquippedSet;
        }

        public async Task<EquippedSet> GetEquippedSetById(int id)
        {
            EquippedSet equippedSet = await this.context.EquippedSets.FindAsync(id);

            return equippedSet;
        }

        public async Task EquipHelmet(int id)
        {
            HeroHelmet heroHelmet = await this.GetHeroHelmetById(id);
            EquippedSet equippedSet = await this.GetCurrentHeroEquipedSet();

            equippedSet.Helmet = heroHelmet;

            await this.context.SaveChangesAsync();
        }

        public async Task EquipArmor(int id)
        {
            HeroArmor heroArmor = await this.GetHeroArmorById(id);
            EquippedSet equippedSet = await this.GetCurrentHeroEquipedSet();

            equippedSet.Armor = heroArmor;

            await this.context.SaveChangesAsync();
        }

        public async Task EquipWeapon(int id)
        {
            HeroWeapon heroWeapon = await this.GetHeroWeaponById(id);
            EquippedSet equippedSet = await this.GetCurrentHeroEquipedSet();

            equippedSet.Weapon = heroWeapon;

            await this.context.SaveChangesAsync();
        }

        public async Task EquipShield(int id)
        {
            HeroShield heroShield = await this.GetHeroShieldById(id);
            EquippedSet equippedSet = await this.GetCurrentHeroEquipedSet();

            equippedSet.Shield = heroShield;

            await this.context.SaveChangesAsync();
        }

        private async Task<HeroHelmet> GetHeroHelmetById(int id)
        {
            HeroHelmet heroHelmet = await this.context.HeroHelmets.FindAsync(id);

            return heroHelmet;
        }

        private async Task<HeroArmor> GetHeroArmorById(int id)
        {
            HeroArmor heroArmor = await this.context.HeroArmors.FindAsync(id);

            return heroArmor;
        }

        private async Task<HeroWeapon> GetHeroWeaponById(int id)
        {
            HeroWeapon heroWeapon = await this.context.HeroWeapons.FindAsync(id);

            return heroWeapon;
        }

        private async Task<HeroShield> GetHeroShieldById(int id)
        {
            HeroShield heroShield = await this.context.HeroShields.FindAsync(id);

            return heroShield;
        }
    }
}
