namespace MyIMDB.Services.Helpers
{
    
    public interface INotificationServiceFactory
    {
       INotificationService CreateService(NotificationServiceType type);
    }
}
