using System.ComponentModel.DataAnnotations;
namespace EventRegistrationAPI_GeorgeDai.DTOs
{
    public class CreateRegistrationDto
    {
        [Required]
        public string StudentName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
