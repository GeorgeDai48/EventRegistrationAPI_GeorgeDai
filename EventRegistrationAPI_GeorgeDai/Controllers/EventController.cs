using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventRegistrationAPI_GeorgeDai.Services;
using EventRegistrationAPI_GeorgeDai.DTOs;

namespace EventRegistrationAPI_GeorgeDai.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class EventController : ControllerBase
    {

        private readonly IEventService _eventService;  
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/Event
        [HttpGet]
        public async Task<ActionResult<List<EventDto>>> GetAllSync()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events); // Return the list of events with a 200 OK status
        }

        //GET: api/Event/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto?>> GetByIdAsync(int id)
        {
            var eventEntity = await _eventService.GetByIdAsync(id);
            if (eventEntity == null)
            {
                return NotFound(); // Return 404 Not Found if the event is not found
            }
            return Ok(eventEntity); // Return the event with a 200 OK status
        }

        //GET: api/Event/{id}/registrations
        [HttpGet("{id}/registrations")]
        public async Task<ActionResult<List<RegistrationDto>>> GetRegistrationsAsync(int id)
        {
            var registrations = await _eventService.GetRegistrationsAsync(id);
            if (!registrations.Any())
            {
                return NotFound(); // Return 404 Not Found if no registrations are found for the event
            }

            return Ok(registrations); // Return the list of registrations with a 200 OK status
        }

        //POST: api/Event
        [HttpPost]
        public async Task<ActionResult<EventDto?>> CreateAsync(CreateEventDto dto)
        {
            var createdEvent = await _eventService.CreateAsync(dto);
            if (createdEvent == null)
            {
                return BadRequest(); // Return 400 Bad Request if the event creation fails
            }
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdEvent.EventId }, createdEvent); // Return 201 Created with the location of the new event
        }

        //POST: api/Event/{id}/register
        [HttpPost("{id}/register")]
        public async Task<ActionResult<EventDto?>> RegisterForEventAsync(int id, CreateRegistrationDto dto)
        {
            var updatedEvent = await _eventService.RegisterForEventAsync(id, dto);
            if (updatedEvent == null)
            {
                return BadRequest(); // Return 400 Bad Request if the registration fails
            }
            return Ok(updatedEvent); // Return the updated event with a 200 OK status
        }
    }
}
