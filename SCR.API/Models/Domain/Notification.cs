// Notification.cs
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SCR.API.Models.Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int StudentId { get; set; }
        public int MaterialId { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Add other properties as needed

        // Navigation properties
        public Student Student { get; set; }
        public Material Material { get; set; }
    }
}
