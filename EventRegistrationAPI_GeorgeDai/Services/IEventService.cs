using EventRegistrationAPI_GeorgeDai.DTOs;

namespace EventRegistrationAPI_GeorgeDai.Services
{
    public interface IEventService
    {
        Task<List<EventDto>> GetAllAsync();
        Task<EventDto?> GetByIdAsync(int id);
        Task<EventDto?> CreateAsync(CreateEventDto dto);
        Task<List<RegistrationDto>> GetRegistrationsAsync(int eventId);
        Task<bool> UpdateAsync(int id, CreateEventDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
