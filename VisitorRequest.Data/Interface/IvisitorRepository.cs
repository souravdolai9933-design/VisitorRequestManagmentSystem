using VisitorRequest.Core.common;
using VisitorRequest.Dto;

namespace VisitorRequest.Interface
{
    public interface IvisitorRepository
    {


        // Create Visitor Request
        Task<DbResult> CreateVisitorRequest(VisitorRequestDto visitorReq);

        // update Visitor Request
        Task<DbResult> UpdateVisitorRequest(UpdateVisitorRequestDto dto);

        // Get All Pending Request
        Task<List<PendingVisitorRequestDto>> GetPendingVisitorRequests();

        // Approved leave Request
        Task<DbResult> ApproveVisitorRequest(ApproveVisitorRequestDto dto);

        // Deleted Visitor Request
        Task<DbResult> DeleteVisitorRequest(int visitorRequestId);

        // Login User
        Task<AppUserDto?> LoginUser(LoginDto dto);

        // Get requests created by a specific user
        Task<List<PendingVisitorRequestDto>> GetMyVisitorRequests(int userId);

        // Reject visitor request
        Task<DbResult> RejectVisitorRequest(int visitorRequestId, int adminId, string remarks);

        // Get all visitor requests in the system (for Admin)
        Task<List<PendingVisitorRequestDto>> GetAllVisitorRequests();

    }
}
