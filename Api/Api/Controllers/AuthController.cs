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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IPersonService personService, IAuthService authService, ILogger<AuthController> logger)
        {
            _personService = personService;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt started for user: {UserName}", request?.UserName);
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogWarning("Login failed: Missing credentials in request.");
                return BadRequest("יש להזין שם משתמש וסיסמה");
            }
            try
            {
                var token = await _personService.LoginAsync(request.UserName, request.Password);
                if (token == null)
                {
                    _logger.LogWarning("Invalid login attempt for user: {UserName}", request.UserName);
                    return Unauthorized(new { message = "שם משתמש או סיסמה שגויים" });
                }
                _logger.LogInformation("User {UserName} logged in successfully.", request.UserName);
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user {UserName}", request.UserName);
                return StatusCode(500, "שגיאה פנימית בשרת");
            }
        }
    }
}