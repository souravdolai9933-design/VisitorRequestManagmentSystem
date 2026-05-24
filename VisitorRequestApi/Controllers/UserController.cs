using Microsoft.AspNetCore.Mvc;
using VisitorRequest.Core.common;
using VisitorRequest.Core.Entitys;
using VisitorRequest.Data.Interface;
using VisitorRequest.Data.Repository;

namespace VisitorRequestApi.Controllers
{

    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {

        private readonly IuserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IuserRepository iuserRepository, ILogger<UserController> logger)
        {
            _userRepository = iuserRepository;
            _logger = logger;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // convert plain password to hash before saving to database
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);


                var result = await _userRepository.CreateUser(user);

                if (result.Result == 1)
                {
                    _logger.LogInformation
                    (
                        "User created successfully with Email : {Email}",
                        user.EmailId
                    );

                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError
                (
                    ex,
                    "Error occurred while creating user with Email : {Email}",
                    user.EmailId
                );

                return StatusCode(500, new DbResult
                {
                    Result = 0,
                    Message = "Internal server error"
                });
            }
        }
    }

}

