using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisitorRequest.Core.Entitys
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$",
            ErrorMessage = "Full name should contain only alphabets")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
            ErrorMessage = "Email format is invalid")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[0-9]{10}$",
            ErrorMessage = "Mobile number must be exactly 10 digits")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 100 characters")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        [Range(1, int.MaxValue,
            ErrorMessage = "RoleId must be greater than 0")]
        public int RoleId { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
