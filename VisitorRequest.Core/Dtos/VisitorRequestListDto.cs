using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorRequest.Core.Dtos
{
    public class VisitorRequestListDto
    {
        public int VisitorRequestId { get; set; }

        public string VisitorName { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string PersonToMeet { get; set; } = string.Empty;

        public string PurposeOfVisit { get; set; } = string.Empty;

        public DateTime VisitDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public string CreatedByName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedByName { get; set; }
    }
}
