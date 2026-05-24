using System.ComponentModel.DataAnnotations;

namespace VisitorRequest.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]

        [StringLength(20, MinimumLength = 6,
         ErrorMessage = "Password must be between 6 and 20 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
