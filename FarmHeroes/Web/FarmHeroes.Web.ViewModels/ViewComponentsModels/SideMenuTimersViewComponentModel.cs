namespace FarmHeroes.Web.ViewModels.ViewComponentsModels
{
    using System;

    public class SideMenuTimersViewComponentModel
    {
        public DateTime? WorkUntil { get; set; }
        
        public DateTime? CannotAttackHeroUntil { get; set; }

        public DateTime? CannotBeAttackedUntil { get; set; }
    }
}
