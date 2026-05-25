namespace VisitorRequest.Dto
{
    public class PendingVisitorRequestDto
    {
        public int VisitorRequestId { get; set; }

        public string VisitorName { get; set; }

        public string MobileNumber { get; set; }

        public string CompanyName { get; set; }

        public string PersonToMeet { get; set; }

        public string PurposeOfVisit { get; set; }

        public DateTime VisitDate { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedByName { get; set; }

    }
}
