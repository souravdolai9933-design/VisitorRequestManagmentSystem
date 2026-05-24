
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using VisitorRequest.Core.common;
using VisitorRequest.Dto;
using VisitorRequest.Interface;
using VisitorRequestApi.Connection;
using VisitorRequestEntity = VisitorRequest.Core.Entitys.VisitorRequest;

namespace VisitorRequest.Repository
{
    public class InitiatorRepository : IInitiatorRepository
    {

        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<InitiatorRepository> _logger;

        public InitiatorRepository(DbConnectionFactory dbConnectionFactory, ILogger<InitiatorRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }


        // Create Visitor Request
        public async Task<DbResult> CreateVisitorRequest(VisitorRequestEntity visitorReq)
        {
            _logger.LogInformation("CreateVisitorRequest: Repository method called for Visitor: {VisitorName}", visitorReq.VisitorName);
            try
            {
                // Create Database Connection
                using var connections = _dbConnectionFactory.CreateConnection();

                DynamicParameters parameter = new DynamicParameters();

                parameter.Add("@VisitorName", visitorReq.VisitorName);
                parameter.Add("@MobileNumber", visitorReq.MobileNumber);
                parameter.Add("@CompanyName", visitorReq.CompanyName);
                parameter.Add("@PersonToMeet", visitorReq.PersonToMeet);
                parameter.Add("@PurposeOfVisit", visitorReq.PurposeOfVisit);
                parameter.Add("@VisitDate", visitorReq.VisitDate);
                parameter.Add("@CreatedBy", visitorReq.CreatedBy);

                var result = await connections.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateVisitorRequest",
                    parameter,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation("CreateVisitorRequest: SP executed successfully. Result: {Result}", result?.Result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateVisitorRequest: Exception occurred for Visitor: {VisitorName}", visitorReq.VisitorName);
                return new DbResult
                {
                    Result = 0,
                    Message = ex.Message
                };
            }
        }

        // Update Visitor Request
        public async Task<DbResult> UpdateVisitorRequest(UpdateVisitorRequestDto dto)
        {
            _logger.LogInformation("UpdateVisitorRequest: Repository method called for Id: {Id}", dto.VisitorRequestId);
            try
            {
                using var connections = _dbConnectionFactory.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@VisitorRequestId", dto.VisitorRequestId);
                parameters.Add("@VisitorName", dto.VisitorName);
                parameters.Add("@MobileNumber", dto.MobileNumber);
                parameters.Add("@CompanyName", dto.CompanyName);
                parameters.Add("@PersonToMeet", dto.PersonToMeet);
                parameters.Add("@PurposeOfVisit", dto.PurposeOfVisit);
                parameters.Add("@VisitDate", dto.VisitDate);
                parameters.Add("@ModifiedBy", dto.ModifiedBy);

                var result = await connections.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateVisitorRequest",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("UpdateVisitorRequest: SP executed successfully for Id: {Id}. Result: {Result}", dto.VisitorRequestId, result?.Result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateVisitorRequest: Exception occurred for Id: {Id}", dto.VisitorRequestId);
                return new DbResult
                {
                    Result = 0,
                    Message = ex.Message
                };

            }
        }

        public async Task<DbResult> DeleteVisitorRequest(int visitorRequestId, int initiatorId)
        {
            _logger.LogInformation(
                "DeleteVisitorRequest: Repository method called for VisitorRequestId: {VisitorRequestId}, InitiatorId: {InitiatorId}",
                visitorRequestId,
                initiatorId);

            using var connections = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@InitiatorId", initiatorId);
            parameters.Add("@VisitorRequestId", visitorRequestId);

            var result = await connections.QueryFirstOrDefaultAsync<DbResult>(
                "sp_DeletePendingRequestByInitiatorId",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation(
                "DeleteVisitorRequest: SP executed for VisitorRequestId: {VisitorRequestId}. Result: {Result}",
                visitorRequestId,
                result?.Result);

            return result ?? new DbResult
            {
                Result = 0,
                Message = "No response received from database"
            };
        }



        // Login User 

        public async Task<AppUserDto?> LoginUser(LoginDto dto)
        {
            _logger.LogInformation("LoginUser: Repository method called for Email: {Email}", dto.Email);
            using var con = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserEmail", dto.Email);

            var user = await con.QueryFirstOrDefaultAsync<AppUserDto>(
                "sp_GetUserByEmail",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (user == null)
            {
                _logger.LogWarning("LoginUser: No user found for Email: {Email}", dto.Email);
                return null;
            }

            // Verrify Password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.LoginPassword);

            if (!isPasswordValid)
            {
                _logger.LogWarning("LoginUser: Invalid password for Email: {Email}", dto.Email);
                return null;
            }

            _logger.LogInformation("LoginUser: Login successful for Email: {Email}", dto.Email);
            return user;
        }



        public async Task<List<PendingVisitorRequestDto>> GetMyVisitorRequests(int userId)
        {
            _logger.LogInformation("GetMyVisitorRequests: Repository method called for UserId: {UserId}", userId);
            using var connections = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            var result = await connections.QueryAsync<PendingVisitorRequestDto>(
                "sp_GetMyVisitorRequests",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("GetMyVisitorRequests: Fetched {Count} records for UserId: {UserId}", result.AsList().Count, userId);
            return result.ToList();
        }
    }
}

