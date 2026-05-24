using Microsoft.AspNetCore.Mvc;
using VisitorRequest.Dto;
using VisitorRequest.Interface;
using Microsoft.AspNetCore.Authorization;
using VisitorRequest.Core.common;

namespace VisitorRequestApi.Controllers
{
    [ApiController]
    [Route("api/Visitor")]
    [Authorize]
    public class VisitorRequestController : ControllerBase
    {
        private readonly IvisitorRepository _visitorRepository;

        public VisitorRequestController(IvisitorRepository visitorRepository)
        {
            _visitorRepository = visitorRepository;
        }

        [HttpPost("addRequest")]
        [Authorize(Roles = "Initiator")]
        public async Task<IActionResult> CreateVisitorRequst([FromBody] VisitorRequestDto visitorReq)
        {
            try
            {
                if (visitorReq == null)
                {
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

                var result = await _visitorRepository.CreateVisitorRequest(visitorReq);

                if (result.Result == 1)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DbResult
                {
                    Result = 0,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Initiator")]
        public async Task<IActionResult> UpdateVisitorRequest(
            [FromRoute] int id,
            [FromBody] UpdateVisitorRequestDto dto)
        {
            if (dto == null)
            {
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

            var result = await _visitorRepository.UpdateVisitorRequest(dto);

            if (result.Result == 1)
            {
                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }

            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingVisitorRequests()
        {
            var result = await _visitorRepository.GetPendingVisitorRequests();
            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVisitorRequests()
        {
            var result = await _visitorRepository.GetAllVisitorRequests();
            return Ok(result);
        }

        [HttpGet("myrequests/{userId}")]
        [Authorize(Roles = "Initiator")]
        public async Task<IActionResult> GetMyVisitorRequests([FromRoute] int userId)
        {
            var result = await _visitorRepository.GetMyVisitorRequests(userId);
            return Ok(result);
        }

        [HttpPost("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveVisitorRequest(
            [FromRoute] int id,
            [FromBody] ApproveVisitorRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Approval data is required"
                });
            }

            dto.VisitorRequestId = id;

            var userId = User.FindFirst("UserId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                dto.AdminId = Convert.ToInt32(userId);
            }
            else if (dto.AdminId <= 0)
            {
                dto.AdminId = 1;
            }

            var result = await _visitorRepository.ApproveVisitorRequest(dto);

            if (result.Result == 1)
            {
                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }

            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }

        [HttpPost("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectVisitorRequest(
            [FromRoute] int id,
            [FromBody] RejectDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Remarks))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Remarks are required for rejection"
                });
            }

            var userId = User.FindFirst("UserId")?.Value;
            int adminId = !string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : 1;

            var result = await _visitorRepository.RejectVisitorRequest(
                id,
                adminId,
                dto.Remarks
            );

            if (result.Result == 1)
            {
                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }

            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Initiator")]
        public async Task<IActionResult> DeleteVisitorRequest([FromRoute] int id)
        {
            var result = await _visitorRepository.DeleteVisitorRequest(id);

            if (result.Result == 1)
            {
                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }

            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }
    }
}