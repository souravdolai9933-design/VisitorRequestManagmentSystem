
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using VisitorRequest.Core.common;
using VisitorRequest.Dto;
using VisitorRequest.Interface;
using VisitorRequestApi.Connection;

namespace VisitorRequest.Repository
{
    public class AdminRepository : IAdminRepository
    {

        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<AdminRepository> _logger;

        public AdminRepository(DbConnectionFactory dbConnectionFactory, ILogger<AdminRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        // Get Pending Visitor Requests
        public async Task<List<PendingVisitorRequestDto>> GetPendingVisitorRequests()
        {
            _logger.LogInformation("GetPendingVisitorRequests: Repository method called.");
            using var connections = _dbConnectionFactory.CreateConnection();

            var result = await connections.QueryAsync<PendingVisitorRequestDto>(
                "sp_GetPendingVisitorRequests",
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("GetPendingVisitorRequests: Fetched {Count} pending records.", result.AsList().Count);
            return result.ToList();
        }


        // Approve Visitor Request
        public async Task<DbResult> ApproveVisitorRequest(int visitorRequestId, int adminId)
        {
            _logger.LogInformation(
                "ApproveVisitorRequest: Repository method called for VisitorRequestId: {VisitorRequestId}, AdminId: {AdminId}",
                visitorRequestId,
                adminId);

            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@VisitorRequestId", visitorRequestId);
            parameters.Add("@AdminId", adminId);

            var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                "sp_ApproveVisitorRequest",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation(
                "ApproveVisitorRequest: SP executed for VisitorRequestId: {VisitorRequestId}. Result: {Result}",
                visitorRequestId,
                result?.Result);

            return result ?? new DbResult
            {
                Result = 0,
                Message = "No response received from database"
            };
        }


        public async Task<DbResult> RejectVisitorRequest(int visitorRequestId, int adminId, string remarks)
        {
            _logger.LogInformation("RejectVisitorRequest: Repository method called for Id: {Id}, AdminId: {AdminId}", visitorRequestId, adminId);
            try
            {
                using var connections = _dbConnectionFactory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@VisitorRequestId", visitorRequestId);
                parameters.Add("@AdminId", adminId);
                parameters.Add("@Remarks", remarks);

                var result = await connections.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_RejectVisitorRequest",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("RejectVisitorRequest: SP executed for Id: {Id}. Result: {Result}", visitorRequestId, result?.Result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RejectVisitorRequest: Exception occurred for Id: {Id}", visitorRequestId);
                return new DbResult
                {
                    Result = 0,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<PendingVisitorRequestDto>> GetAllVisitorRequests()
        {
            _logger.LogInformation("GetAllVisitorRequests: Repository method called.");
            using var connections = _dbConnectionFactory.CreateConnection();

            var result = await connections.QueryAsync<PendingVisitorRequestDto>(
                "sp_GetAllVisitorRequests",
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("GetAllVisitorRequests: Fetched {Count} total records.", result.AsList().Count);
            return result.ToList();
        }
    }
}

