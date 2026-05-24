using VisitorRequest.Core.common;
using VisitorRequest.Dto;
using VisitorRequestEntity = VisitorRequest.Core.Entitys.VisitorRequest;

namespace VisitorRequest.Interface
{
    public interface IInitiatorRepository
    {
        // Create Visitor Request
        Task<DbResult> CreateVisitorRequest(VisitorRequestEntity visitorReq);

        // Update Visitor Request
        Task<DbResult> UpdateVisitorRequest(UpdateVisitorRequestDto dto);

        // Deleted Visitor Request
        Task<DbResult> DeleteVisitorRequest(int visitorRequestId, int initiatorId);

        // Login User
        Task<AppUserDto?> LoginUser(LoginDto dto);

        // Get requests created by a specific user
        Task<List<PendingVisitorRequestDto>> GetMyVisitorRequests(int userId);
    }
}
