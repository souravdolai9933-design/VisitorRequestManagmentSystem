using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorRequest.Core.Dtos
{
    public class RejectVisitorRequestDto
    {
        public int VisitorRequestId { get; set; }

        public int AdminId { get; set; }

        public string Remarks { get; set; } = string.Empty;

    }
}
