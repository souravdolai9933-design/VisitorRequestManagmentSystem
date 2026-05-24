using Microsoft.AspNetCore.Mvc;
using VisitorRequest.Dto;
using VisitorRequestApi.Healper;
using VisitorRequest.Interface;

namespace VisitorRequestApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IInitiatorRepository _initiatorRepository;
        private readonly JwtHealper _jwtHealper;

        public AuthController(IInitiatorRepository initiatorRepository, JwtHealper jwtHealper)
        {
            _initiatorRepository = initiatorRepository;
            _jwtHealper = jwtHealper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Login data is required"
                });
            }

            var user = await _initiatorRepository.LoginUser(dto);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid email or password"
                });
            }
            // Generate JWT token
            var token = _jwtHealper.GenerateToken(user);

            return Ok(new
            {
                success = true,
                message = "Login successful",
                data = new
                {
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.RoleId,
                    user.RoleName,
                    token
                }
            });
        }
    }
}