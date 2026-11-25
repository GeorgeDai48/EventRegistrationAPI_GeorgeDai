using EventRegistrationAPI_GeorgeDai.DTOs;
using EventRegistrationAPI_GeorgeDai.Data;
using EventRegistrationAPI_GeorgeDai.Models;
using Microsoft.EntityFrameworkCore;
namespace EventRegistrationAPI_GeorgeDai.Services
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EventDto>> GetAllAsync()
        {
            return await _context.Events
                .Select(e => new EventDto
                {
                    EventId = e.EventId,
                    Title = e.Title,
                    Location = e.Location,
                    EventDate = e.EventDate,
                    Capacity = e.Capacity,
                    RegisteredCount = e.Registrations.Count(),
                    SpotsLeft = e.Capacity - e.Registrations.Count()
                })
                .ToListAsync();
        }

        public async Task<EventDto?> GetByIdAsync(int id)
        {
            var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);
            if (eventEntity == null)
            {
                return null;
            }
            return new EventDto
            {
                EventId = eventEntity.EventId,
                Title = eventEntity.Title,
                Location = eventEntity.Location,
                EventDate = eventEntity.EventDate,
                Capacity = eventEntity.Capacity,
                RegisteredCount = eventEntity.Registrations.Count(),
                SpotsLeft = eventEntity.Capacity - eventEntity.Registrations.Count()
            };
        }

        public async Task<EventDto?> CreateAsync(CreateEventDto dto)
        {
            var newEvent = new Event
            {
                Title = dto.Title,
                Location = dto.Location,
                EventDate = dto.EventDate,
                Capacity = dto.Capacity
            };
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return new EventDto
            {
                EventId = newEvent.EventId,
                Title = newEvent.Title,
                Location = newEvent.Location,
                EventDate = newEvent.EventDate,
                Capacity = newEvent.Capacity,
                RegisteredCount = 0,
                SpotsLeft = newEvent.Capacity
            };
        }

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

        public async Task<EventDto?> RegisterForEventAsync(int eventId, CreateRegistrationDto dto){
            var eventEntity = await _context.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.EventId == eventId);
            if (eventEntity == null || eventEntity.Registrations.Count >= eventEntity.Capacity)
            {
                return null; // Event not found or full
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
            return await GetByIdAsync(eventId); // Return the updated event details
        }

    }
}
