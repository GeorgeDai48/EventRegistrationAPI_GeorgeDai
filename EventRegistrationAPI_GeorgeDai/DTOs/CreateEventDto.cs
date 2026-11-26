using System.ComponentModel.DataAnnotations;
using EventRegistrationAPI_GeorgeDai.Validation;
namespace EventRegistrationAPI_GeorgeDai.DTOs
{
    public class CreateEventDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }
        [Required]
        public string Location { get; set; }
        [FutureDate(ErrorMessage = "Event date must be in the future.")]
        public DateTime EventDate { get; set; }
        [Required]
        [Range(1, 500, ErrorMessage = "Capacity must be between 1 and 500.")]
        public int Capacity { get; set; }
    }
}
