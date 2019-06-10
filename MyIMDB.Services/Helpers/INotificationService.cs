using System.Threading.Tasks;

namespace MyIMDB.Services.Helpers
{
    public interface INotificationService
    {
        Task Send(NotificationMessage message);
    }
}
