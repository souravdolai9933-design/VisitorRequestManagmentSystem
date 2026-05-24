using System.ComponentModel.DataAnnotations;

namespace VisitorRequest.Dto
{
    public class VisitorRequestDto
    {
        public int VisitorRequestId { get; set; }

        [Required(ErrorMessage = "Visitor name is required")]
        [StringLength(100, ErrorMessage = "Visitor name cannot exceed 100 characters")]
        public string VisitorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^\d{10}$",
            ErrorMessage = "Mobile number must be exactly 10 digits")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(150, ErrorMessage = "Company name cannot exceed 150 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Person to meet is required")]
        [StringLength(100, ErrorMessage = "Person name cannot exceed 100 characters")]
        public string PersonToMeet { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purpose of visit is required")]
        [StringLength(250, ErrorMessage = "Purpose cannot exceed 250 characters")]
        public string PurposeOfVisit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Visit date is required")]
        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }

        public string Status { get; set; } = "Pending";

        public int CreatedBy { get; set; }

        public string? Remarks { get; set; }

        public string? CreatedByName { get; set; }
    }
}
