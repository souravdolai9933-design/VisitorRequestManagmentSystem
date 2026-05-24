using System.ComponentModel.DataAnnotations;

namespace VisitorRequest.Dto
{
    public class RejectDto
    {
        [Required]
        public string Remarks { get; set; } = string.Empty;
    }
}
