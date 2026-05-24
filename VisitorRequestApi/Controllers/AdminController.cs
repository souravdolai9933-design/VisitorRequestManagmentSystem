using Microsoft.AspNetCore.Mvc;
using VisitorRequest.Dto;
using VisitorRequest.Interface;
using Microsoft.AspNetCore.Authorization;
using VisitorRequest.Core.common;


namespace VisitorRequestApi.Controllers
{
    [ApiController]
    [Route("api/Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminRepository adminRepository, ILogger<AdminController> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingVisitorRequests()
        {
            _logger.LogInformation("GetPendingVisitorRequests: Method called.");
            var result = await _adminRepository.GetPendingVisitorRequests();
            _logger.LogInformation("GetPendingVisitorRequests: Returned {Count} pending requests.", result.Count);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllVisitorRequests()
        {
            _logger.LogInformation("GetAllVisitorRequests: Method called.");
            var result = await _adminRepository.GetAllVisitorRequests();
            _logger.LogInformation("GetAllVisitorRequests: Returned {Count} total requests.", result.Count);
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveVisitorRequest([FromRoute] int id)
        {
            _logger.LogInformation(
                "ApproveVisitorRequest: Method called for VisitorRequestId: {VisitorRequestId}",
                id);

            try
            {
                var userId = User.FindFirst("UserId")?.Value
                             ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("ApproveVisitorRequest: UserId not found in JWT token");

                    return Unauthorized(new
                    {
                        success = false,
                        message = "Invalid token. User id not found."
                    });
                }

                int adminId = Convert.ToInt32(userId);

                var result = await _adminRepository.ApproveVisitorRequest(id, adminId);

                if (result.Result == 1)
                {
                    _logger.LogInformation(
                        "ApproveVisitorRequest: Visitor request Id: {VisitorRequestId} approved successfully by AdminId: {AdminId}",
                        id,
                        adminId);

                    return Ok(new
                    {
                        success = true,
                        message = result.Message
                    });
                }

                _logger.LogWarning(
                    "ApproveVisitorRequest: Failed to approve VisitorRequestId: {VisitorRequestId}. Message: {Message}",
                    id,
                    result.Message);

                return BadRequest(new
                {
                    success = false,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "ApproveVisitorRequest: Exception occurred while approving VisitorRequestId: {VisitorRequestId}",
                    id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error"
                });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectVisitorRequest(
     [FromRoute] int id,
     [FromBody] RejectDto dto)
        {
            _logger.LogInformation(
                "RejectVisitorRequest: Method called for VisitorRequestId: {VisitorRequestId}",
                id);

            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Remarks))
                {
                    _logger.LogWarning(
                        "RejectVisitorRequest: Remarks are missing for VisitorRequestId: {VisitorRequestId}",
                        id);

                    return BadRequest(new
                    {
                        success = false,
                        message = "Remarks are required for rejection"
                    });
                }

                var userId = User.FindFirst("UserId")?.Value
                             ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("RejectVisitorRequest: UserId not found in JWT token");

                    return Unauthorized(new
                    {
                        success = false,
                        message = "Invalid token. User id not found."
                    });
                }

                int adminId = Convert.ToInt32(userId);

                var result = await _adminRepository.RejectVisitorRequest(
                    id,
                    adminId,
                    dto.Remarks.Trim()
                );

                if (result.Result == 1)
                {
                    _logger.LogInformation(
                        "RejectVisitorRequest: Visitor request Id: {VisitorRequestId} rejected successfully by AdminId: {AdminId}",
                        id,
                        adminId);

                    return Ok(new
                    {
                        success = true,
                        message = result.Message
                    });
                }

                _logger.LogWarning(
                    "RejectVisitorRequest: Failed to reject VisitorRequestId: {VisitorRequestId}. Message: {Message}",
                    id,
                    result.Message);

                return BadRequest(new
                {
                    success = false,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "RejectVisitorRequest: Exception occurred while rejecting VisitorRequestId: {VisitorRequestId}",
                    id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error"
                });
            }
        }
    }
}
