using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventRegistrationAPI_GeorgeDai.Models
{
    // Represents a registration for an event.
    public class Registration
    {
        public int RegistrationId { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        [Required]
        public string StudentName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime RegisteredOn { get; set; }
        public Event Event { get; set; }
    }
}
