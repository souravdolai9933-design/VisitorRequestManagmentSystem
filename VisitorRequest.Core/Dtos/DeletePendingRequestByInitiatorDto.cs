using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorRequest.Core.Dtos
{
    public class DeletePendingRequestByInitiatorDto
    {
        public int InitiatorId { get; set; }

        public int VisitorRequestId { get; set; }

    }
}
