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

        //Registers a student for an event and checking for business rules
        public async Task<RegisterEventResult> RegisterForEventAsync(int eventId, CreateRegistrationDto dto)
        {
            var eventEntity = await _context.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.EventId == eventId);
            //business logic to check if the event exists, if it's full, if the event date has passed, and if the user is already registered
            if (eventEntity == null)
            {
                return new RegisterEventResult { ResultType = "EventNotFound" }; // Event not found 
            }
            if (eventEntity.Registrations.Count() >= eventEntity.Capacity)
            {
                return new RegisterEventResult { ResultType = "EventFull" }; // Event is full
            }
            if (eventEntity.EventDate < DateTime.Now)
            {
                return new RegisterEventResult { ResultType = "EventAlreadyHappened" }; // Event already happened
            }
            if (eventEntity.Registrations.Any(r => r.Email == dto.Email))
            {
                return new RegisterEventResult { ResultType = "AlreadyRegistered" }; // Already registered
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
            return new RegisterEventResult
            {
                ResultType = "Success",
                EventId = eventId   // Return the event ID for the successfully registered event
            }; 
        }

        // Retrieves all registrations for a specific event 
        public async Task<List<RegistrationDto>> GetRegistrationsAsync(int eventId)
        {
            return await _context.Registrations
                .Where(r => r.EventId == eventId)
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
        //Deletes a registration by its ID 
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
