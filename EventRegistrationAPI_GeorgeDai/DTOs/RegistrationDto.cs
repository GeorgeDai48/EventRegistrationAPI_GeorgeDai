using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_GeorgeDai.DTOs
{
    public class RegistrationDto
    {
        public int RegistrationId { get; set; }
        public string EventTitle { get; set; }
        [Required]
        public string StudentName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime RegisteredOn { get; set; }
    }
}
