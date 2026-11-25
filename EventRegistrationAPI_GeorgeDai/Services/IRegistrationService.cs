using EventRegistrationAPI_GeorgeDai.DTOs;

namespace EventRegistrationAPI_GeorgeDai.Services
{
    public interface IRegistrationService
    {
        Task<bool> DeleteAsync(int id);
    }
}
