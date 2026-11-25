using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventRegistrationAPI_GeorgeDai.Services;
using EventRegistrationAPI_GeorgeDai.DTOs;

namespace EventRegistrationAPI_GeorgeDai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // DELETE: api/Registration/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _registrationService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(); // Return 404 Not Found if the registration is not found
            }
            return NoContent(); // Return 204 No Content if the deletion is successful
        }

    }
}
