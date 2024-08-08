using SCR.API.Models.Domain;
using SCR.API.Models.DTO;

namespace SCR.API.Services
{
    public class NotificationService
    {
        public NotificationDTO CreateNotificationDTO(Notification notification)
        {
            return new NotificationDTO
            {
                NotificationId = notification.NotificationId,
                StudentId = notification.StudentId,
                MaterialId = notification.MaterialId,
                Description = notification.Description,
                Timestamp = notification.Timestamp
                // Add other properties as needed
            };
        }

        // Additional methods for handling notifications (e.g., sending notifications)
    }
}
