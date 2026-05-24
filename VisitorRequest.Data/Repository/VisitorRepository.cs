
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using VisitorRequest.Core.common;
using VisitorRequest.Data.Repository;
using VisitorRequest.Dto;
using VisitorRequest.Interface;
using VisitorRequestApi.Connection;

namespace VisitorRequest.Repository
{
    public class VisitorRepository : IvisitorRepository
    {

        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<UserRepository> _logger;

        public VisitorRepository(DbConnectionFactory dbConnectionFactory,ILogger<UserRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }


        // Create Visitor Request
        public async Task<DbResult> CreateVisitorRequest(VisitorRequestDto visitorReq)
        {
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

                return result;
            }
            catch (Exception ex)
            {
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

                return result;
            }
            catch (Exception ex)
            {
                return new DbResult
                {
                    Result = 0,
                    Message = ex.Message
                };

            }
        }

        // Get Pending Visitor Requests
        public async Task<List<PendingVisitorRequestDto>> GetPendingVisitorRequests()
        {
            using var connections = _dbConnectionFactory.CreateConnection();

            var result = await connections.QueryAsync<PendingVisitorRequestDto>(
                "sp_GetPendingVisitorRequests",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        // Approve Visitor Request
        public async Task<DbResult> ApproveVisitorRequest(ApproveVisitorRequestDto dto)
        {
            using var connections = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@VisitorRequestId", dto.VisitorRequestId);
            parameters.Add("@AdminId", dto.AdminId);

            var result = await connections.QueryFirstOrDefaultAsync<DbResult>(
                "sp_ApproveVisitorRequest",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }




        
        public async Task<DbResult> DeleteVisitorRequest(int visitorRequestId)
        {
            using var connections = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@VisitorRequestId", visitorRequestId);

            var result = await connections.QueryFirstOrDefaultAsync<DbResult>(
                "sp_DeleteVisitorRequest",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<AppUser> LoginUser(LoginDto dto)
        {
            using var con = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Email", dto.Email);

            parameters.Add("@Password", dto.Password);

            var user = await con.QueryFirstOrDefaultAsync<AppUser>(
                "sp_LoginUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return user;
        }

        public async Task<List<PendingVisitorRequestDto>> GetMyVisitorRequests(int userId)
        {
            using var connections = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            var result = await connections.QueryAsync<PendingVisitorRequestDto>(
                "sp_GetMyVisitorRequests",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<DbResult> RejectVisitorRequest(int visitorRequestId, int adminId, string remarks)
        {
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

                return result;
            }
            catch (Exception ex)
            {
                return new DbResult
                {
                    Result = 0,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<PendingVisitorRequestDto>> GetAllVisitorRequests()
        {
            using var connections = _dbConnectionFactory.CreateConnection();

            var result = await connections.QueryAsync<PendingVisitorRequestDto>(
                "sp_GetAllVisitorRequests",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
    }
}

