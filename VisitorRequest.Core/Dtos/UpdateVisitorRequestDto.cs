using System.ComponentModel.DataAnnotations;

namespace VisitorRequest.Dto
{
    public class UpdateVisitorRequestDto
    {
            [Required(ErrorMessage = "Visitor request id is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Invalid visitor request id")]
            public int VisitorRequestId { get; set; }


            [Required(ErrorMessage = "Visitor name is required")]
            [StringLength(100, ErrorMessage = "Visitor name cannot exceed 100 characters")]
            public string VisitorName { get; set; } = string.Empty;


            [Required(ErrorMessage = "Mobile number is required")]
            [RegularExpression(@"^[0-9]{10}$",
                ErrorMessage = "Mobile number must contain exactly 10 numeric digits")]
            public string MobileNumber { get; set; } = string.Empty;


            [Required(ErrorMessage = "Company name is required")]
            [StringLength(150, ErrorMessage = "Company name cannot exceed 150 characters")]
            public string CompanyName { get; set; } = string.Empty;


            [Required(ErrorMessage = "Person to meet is required")]
            [StringLength(100, ErrorMessage = "Person to meet cannot exceed 100 characters")]
            public string PersonToMeet { get; set; } = string.Empty;


            [Required(ErrorMessage = "Purpose of visit is required")]
            [StringLength(250, ErrorMessage = "Purpose of visit cannot exceed 250 characters")]
            public string PurposeOfVisit { get; set; } = string.Empty;


            [Required(ErrorMessage = "Visit date is required")]
            [DataType(DataType.Date)]
            public DateTime VisitDate { get; set; }


           
            public int ModifiedBy { get; set; }
        }
    }

