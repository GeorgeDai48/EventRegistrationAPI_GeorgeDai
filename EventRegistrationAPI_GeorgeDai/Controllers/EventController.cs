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
        private readonly IRegistrationService _registrationService;

        public EventController(IEventService eventService, IRegistrationService registrationService)
        {
            _eventService = eventService;
            _registrationService = registrationService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<List<EventDto>>> GetAllSync()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events); // Return the list of events with a 200 OK status
        }

        //GET: api/Events/{id}
        [HttpGet("{id}", Name = "GetByIdAsync")]
        public async Task<ActionResult<EventDto?>> GetByIdAsync(int id)
        {
            var eventEntity = await _eventService.GetByIdAsync(id);
            if (eventEntity == null)
            {
                return NotFound(); // Return 404 Not Found if the event is not found
            }
            return Ok(eventEntity); // Return the event with a 200 OK status
        }

        //POST: api/Events
        [HttpPost]
        public async Task<ActionResult<EventDto?>> CreateAsync(CreateEventDto dto)
        {
            var createdEvent = await _eventService.CreateAsync(dto);
            if (createdEvent == null)
            {
                return BadRequest(); // Return 400 Bad Request if the event creation fails
            }
            return CreatedAtRoute("GetByIdAsync", new { id = createdEvent.EventId }, createdEvent); // 201 Created with the location of the newly created event
        }

        //GET: api/Events/{id}/registrations
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

        //POST: api/Events/{id}/registrations
        [HttpPost("{id}/registrations")]
        public async Task<ActionResult<EventDto?>> RegisterForEventAsync(int id, CreateRegistrationDto dto)
        {
            // Business rules
            var result = await _registrationService.RegisterForEventAsync(id, dto);
            if (result.ResultType == "EventNotFound")
            {
                return NotFound("Event not found"); // Return 404 Not Found if the event is not found
            }
            if (result.ResultType == "EventAlreadyHappened")
            {
                return BadRequest("Registration is closed for past events"); // Return 400 Bad Request if the event has already happened
            }
            if (result.ResultType == "AlreadyRegistered")
            {
                return Conflict("This email is already registered for this event"); // Return 409 Conflict if the user has already registered for the event
            }
            if (result.ResultType == "EventFull")
            {
                return Conflict("This Event is full"); // Return 409 Conflict if the event is full
            }
            return CreatedAtRoute("GetByIdAsync", new { id = result.EventId }, await _eventService.GetByIdAsync(result.EventId)); // Return the updated event with a 201 Created status
        }
    }
}
