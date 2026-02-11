using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IAuthService _authService;

        public AuthController(IPersonService personService, IAuthService authService)
        {
            _personService = personService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("יש להזין שם משתמש וסיסמה");
            }

            var token = await _personService.LoginAsync(request.UserName, request.Password);

            if (token == null)
            {
                return Unauthorized(new { message = "שם משתמש או סיסמה שגויים" });
            }

            return Ok(new { token = token });
        }
    }
}