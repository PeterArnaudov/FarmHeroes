namespace FarmHeroes.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FarmHeroes.Data.Models.NotificationModels.HeroModels;

    public interface INotificationService
    {
        Task<TViewModel> GetAllNotifications<TViewModel>();

        Task<int> GetNewNotificationsCount();

        Task AddNotification(Notification notification);

        Task DeleteOld();
    }
}
