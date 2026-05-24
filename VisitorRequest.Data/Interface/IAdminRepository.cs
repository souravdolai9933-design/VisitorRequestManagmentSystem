using VisitorRequest.Core.common;
using VisitorRequest.Dto;

namespace VisitorRequest.Interface
{
    public interface IAdminRepository
    {
        // Get All Pending Request
        Task<List<PendingVisitorRequestDto>> GetPendingVisitorRequests();

        // Approved Visitor Request
        Task<DbResult> ApproveVisitorRequest(int visitorRequestId, int adminId);

        // Reject visitor request
        Task<DbResult> RejectVisitorRequest(int visitorRequestId, int adminId, string remarks);

        // Get all visitor requests in the system (for Admin)
        Task<List<PendingVisitorRequestDto>> GetAllVisitorRequests();
    }
}
