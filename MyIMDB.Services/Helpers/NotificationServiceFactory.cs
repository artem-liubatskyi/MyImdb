using System;

namespace MyIMDB.Services.Helpers
{
    public class NotificationServiceFactory : INotificationServiceFactory
    {
        public NotificationServiceFactory()
        {
        }

        public INotificationService CreateService(NotificationServiceType type)
        {
            switch (type)
            {
                case NotificationServiceType.Email:
                   return new EmailNotificationService();
                default:
                    throw new NotImplementedException("Notification service does not exist");
            }

            
        }
    }
}