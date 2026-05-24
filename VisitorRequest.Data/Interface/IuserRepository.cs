using System;
using System.Collections.Generic;
using System.Text;
using VisitorRequest.Core.common;
using VisitorRequest.Core.Entitys;

namespace VisitorRequest.Data.Interface
{
    public interface IuserRepository
    {

        // Create User 
        public Task<DbResult> CreateUser(User user);

    }
}
