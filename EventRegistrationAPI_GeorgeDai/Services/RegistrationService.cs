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

        public async Task<List<RegistrationDto>> GetAllAsync()
        {
            return await _context.Registrations
                .Select(r => new RegistrationDto
                {
                    RegistrationId = r.RegistrationId,
                    StudentName = r.StudentName,
                    Email = r.Email,
                    EventTitle = r.Event.Title,
                    RegisteredOn = r.RegisteredOn 
                })
                .ToListAsync();
        }

        public async Task<RegistrationDto?> GetByIdAsync(int id)
        {
            var registration = await _context.Registrations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.RegistrationId == id);
            if (registration == null)
            {
                return null;
            }
            return new RegistrationDto
            {
                RegistrationId = registration.RegistrationId,
                StudentName = registration.StudentName,
                Email = registration.Email,
                EventTitle = registration.Event.Title,
                RegisteredOn = registration.RegisteredOn
            };
        }

        public async Task<RegistrationDto> CreateAsync(int eventId, CreateRegistrationDto dto)
        {
            var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.EventId == eventId);
            var registeredCount = await _context.Registrations.CountAsync(r => r.EventId == eventId);

            if (eventEntity == null || registeredCount >= eventEntity.Capacity)
            {
                return null;
            }
            var newRegistration = new Registration
            {
                StudentName = dto.StudentName,
                Email = dto.Email,
                EventId = eventId,
                RegisteredOn = DateTime.Now
            };
            _context.Registrations.Add(newRegistration);
            await _context.SaveChangesAsync();
            return new RegistrationDto
            {
                RegistrationId = newRegistration.RegistrationId,
                StudentName = newRegistration.StudentName,
                Email = newRegistration.Email,
                EventTitle = eventEntity.Title,
                RegisteredOn = newRegistration.RegisteredOn
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateRegistrationDto dto)
        {
            var registration = await _context.Registrations.FirstOrDefaultAsync(r => r.RegistrationId == id);
            if (registration == null)
            {
                return false;
            }
            registration.StudentName = dto.StudentName;
            registration.Email = dto.Email;
            await _context.SaveChangesAsync();
            return true;
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
