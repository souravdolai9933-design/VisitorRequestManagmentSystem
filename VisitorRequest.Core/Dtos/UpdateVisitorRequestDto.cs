namespace VisitorRequest.Dto
{
    public class UpdateVisitorRequestDto
    {
        public int VisitorRequestId { get; set; }

        public string VisitorName { get; set; }

        public string MobileNumber { get; set; }

        public string CompanyName { get; set; }

        public string PersonToMeet { get; set; }

        public string PurposeOfVisit { get; set; }

        public DateTime VisitDate { get; set; }

        public int ModifiedBy
        {
            get; set;

        }
    }
}
