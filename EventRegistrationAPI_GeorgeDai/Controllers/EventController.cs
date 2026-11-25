using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventRegistrationAPI_GeorgeDai.Services;
using EventRegistrationAPI_GeorgeDai.DTOs;
using EventRegistrationAPI_GeorgeDai.Models;

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

        private readonly IRegistrationService _registrationService;
        public EventController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
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
            var registrations = await _registrationService.GetRegistrationsAsync(id);
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
            var result = await _registrationService.RegisterForEventAsync(id, dto);
            if (result.ResultType == "EventNotFound")
            {
                return NotFound("Event not found"); // Return 404 Not Found if the event is not found
            }
            if (result.ResultType == "EventFull")
            {
                return Conflict("This Event is full"); // Return 409 Conflict if the event is full
            }
            if (result.ResultType == "EventAlreadyHappened")
            {
                return BadRequest("Registration is closed for past events"); // Return 400 Bad Request if the event has already happened
            }
            if (result.ResultType == "AlreadyRegistered")
            {
                return Conflict("This email is already registered for this event"); // Return 409 Conflict if the user has already registered for the event
            }
            return Ok(GetByIdAsync(result.EventId)); // Return the updated event with a 200 OK status
        }
    }
}
