using EventRegistrationAPI_GeorgeDai.DTOs;

namespace EventRegistrationAPI_GeorgeDai.Services
{
    public interface IRegistrationService
    {
        Task<List<RegistrationDto>> GetAllAsync();
        Task<RegistrationDto?> GetByIdAsync(int id);
        Task<RegistrationDto?> CreateAsync(int eventId, CreateRegistrationDto dto);
        Task<bool> UpdateAsync(int id, CreateRegistrationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
