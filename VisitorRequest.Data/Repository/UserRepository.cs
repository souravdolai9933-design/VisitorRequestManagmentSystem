using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VisitorRequest.Core.common;
using VisitorRequest.Core.Entitys;
using VisitorRequest.Data.Interface;
using VisitorRequestApi.Connection;

namespace VisitorRequest.Data.Repository
{
    public class UserRepository : IuserRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DbConnectionFactory dbConnectionFactory, ILogger<UserRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<DbResult> CreateUser(User user)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@FullName", user.FullName);
                parameters.Add("@EmailId", user.EmailId);
                parameters.Add("@MobileNumber", user.MobileNumber);
                parameters.Add("@PasswordHash", user.PasswordHash);
                parameters.Add("@RoleId", user.RoleId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>
                (
                    "sp_Create_User",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating user with Email: {Email}", user.EmailId);
                throw;
            }
        }
    }
}
