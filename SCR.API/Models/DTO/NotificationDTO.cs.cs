namespace SCR.API.Models.DTO
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public int StudentId { get; set; }
        public int MaterialId { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        // Add other properties as needed
    }
}
