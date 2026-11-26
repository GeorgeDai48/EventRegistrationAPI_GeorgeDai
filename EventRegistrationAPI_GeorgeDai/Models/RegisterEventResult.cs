namespace EventRegistrationAPI_GeorgeDai.Models
{
    // Represents the result of an event registration attempt.
    public class RegisterEventResult
    {
        //ResultType for determining the result of the registration attempt and what kind of code (404, 409, etc.) to return.
        public string ResultType { get; set; }
        //EventId for the event that the registration was made for. For getting details when returning 201 Created response.
        public int EventId { get; set; }
    }
}
