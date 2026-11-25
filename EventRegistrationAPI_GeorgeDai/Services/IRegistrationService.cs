using EventRegistrationAPI_GeorgeDai.DTOs;
using EventRegistrationAPI_GeorgeDai.Models;

namespace EventRegistrationAPI_GeorgeDai.Services
{
    public interface IRegistrationService
    {
        Task<RegisterEventResult> RegisterForEventAsync(int eventId, CreateRegistrationDto dto);
        Task<List<RegistrationDto>> GetRegistrationsAsync(int eventId);
        Task<bool> DeleteAsync(int id);
    }
}
