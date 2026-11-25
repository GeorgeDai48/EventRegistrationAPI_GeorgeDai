using EventRegistrationAPI_GeorgeDai.DTOs;
using EventRegistrationAPI_GeorgeDai.Data;
using EventRegistrationAPI_GeorgeDai.Models;
using Microsoft.EntityFrameworkCore;
namespace EventRegistrationAPI_GeorgeDai.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _context;
        public RegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var registration = await _context.Registrations.FirstOrDefaultAsync(r => r.RegistrationId == id);
            if (registration == null)
            {
                return false;
            }
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
