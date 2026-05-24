using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VisitorRequest.Core.common;
using VisitorRequest.Dto;
using VisitorRequest.Interface;
using VisitorRequestApi.Connection;
using VisitorRequestEntity = VisitorRequest.Core.Entitys.VisitorRequest;


namespace VisitorRequestApi.Controllers
{
    [ApiController]
    [Route("api/Initiator")]
    [Authorize(Roles = "Initiator")]
    public class InitiatorController : ControllerBase
    {
        private readonly IInitiatorRepository _initiatorRepository;
        private readonly ILogger<InitiatorController> _logger;


        public InitiatorController(IInitiatorRepository initiatorRepository, ILogger<InitiatorController> logger)
        {
            _initiatorRepository = initiatorRepository;
            _logger = logger;
        }
             

        [HttpPost("addRequest")]
        public async Task<IActionResult> CreateVisitorRequst([FromBody] VisitorRequestEntity visitorReq)
        {
            _logger.LogInformation("CreateVisitorRequest: Method called.");
            try
            {
                if (visitorReq == null)
                {
                    _logger.LogWarning("CreateVisitorRequest: Request body is null.");
                    return BadRequest(new DbResult
                    {
                        Result = 0,
                        Message = "Invalid Request Data"
                    });
                }

                var userId = User.FindFirst("UserId")?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    visitorReq.CreatedBy = Convert.ToInt32(userId);
                }
                else if (visitorReq.CreatedBy <= 0)
                {
                    visitorReq.CreatedBy = 1;
                }

                var result = await _initiatorRepository.CreateVisitorRequest(visitorReq);

                if (result.Result == 1)
                {
                    _logger.LogInformation("CreateVisitorRequest: Visitor request created successfully.");
                    return Ok(result);
                }

                _logger.LogWarning("CreateVisitorRequest: Failed to create visitor request. Message: {Message}", result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateVisitorRequest: Exception occurred.");
                return StatusCode(500, new DbResult
                {
                    Result = 0,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateVisitorRequest(
            [FromRoute] int id,
            [FromBody] UpdateVisitorRequestDto dto)
        {
            _logger.LogInformation("UpdateVisitorRequest: Method called for Id: {Id}", id);
            if (dto == null)
            {
                _logger.LogWarning("UpdateVisitorRequest: Request body is null for Id: {Id}", id);
                return BadRequest(new
                {
                    success = false,
                    message = "Visitor request data is required"
                });
            }

            dto.VisitorRequestId = id;

            var userId = User.FindFirst("UserId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                dto.ModifiedBy = Convert.ToInt32(userId);
            }
            else if (dto.ModifiedBy <= 0)
            {
                dto.ModifiedBy = 1;
            }

            var result = await _initiatorRepository.UpdateVisitorRequest(dto);

            if (result.Result == 1)
            {
                _logger.LogInformation("UpdateVisitorRequest: Visitor request Id: {Id} updated successfully.", id);
                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }

            _logger.LogWarning("UpdateVisitorRequest: Failed to update visitor request Id: {Id}. Message: {Message}", id, result.Message);
            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }

        [HttpGet("myrequests/{userId}")]
        public async Task<IActionResult> GetMyVisitorRequests([FromRoute] int userId)
        {
            _logger.LogInformation("GetMyVisitorRequests: Method called for UserId: {UserId}", userId);
            var result = await _initiatorRepository.GetMyVisitorRequests(userId);
            _logger.LogInformation("GetMyVisitorRequests: Returned {Count} requests for UserId: {UserId}", result.Count, userId);
            return Ok(result);
        }


        [Authorize(Roles = "Initiator")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteVisitorRequest([FromRoute] int id)
        {
            _logger.LogInformation(
                "DeleteVisitorRequest: Method called for VisitorRequestId: {VisitorRequestId}",
                id);

            try
            {
                var userId = User.FindFirst("UserId")?.Value
                             ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("DeleteVisitorRequest: UserId not found in JWT token");

                    return Unauthorized(new
                    {
                        success = false,
                        message = "Invalid token"
                    });
                }

                int initiatorId = Convert.ToInt32(userId);

                var result = await _initiatorRepository.DeleteVisitorRequest(id, initiatorId);

                if (result.Result == 1)
                {
                    _logger.LogInformation(
                        "DeleteVisitorRequest: Visitor request deleted successfully for Id: {VisitorRequestId}",
                        id);

                    return Ok(new
                    {
                        success = true,
                        message = result.Message
                    });
                }

                _logger.LogWarning(
                    "DeleteVisitorRequest: Failed to delete VisitorRequestId: {VisitorRequestId}. Message: {Message}",
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
                    "DeleteVisitorRequest: Exception occurred while deleting VisitorRequestId: {VisitorRequestId}",
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
