using EventRegistrationAPI_GeorgeDai.Models;
using System.ComponentModel.DataAnnotations;
namespace EventRegistrationAPI_GeorgeDai.DTOs
{
    public class EventDto
    {
        public int EventId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }
        [Required]
        public string Location { get; set; }
        public DateTime EventDate { get; set; }
        public int Capacity { get; set; }
        public int RegisteredCount { get; set; }
        public int SpotsLeft { get; set; }
    }
}
