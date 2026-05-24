namespace VisitorRequest.Dto
{
    public class AppUserDto
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string LoginPassword { get; set; } = string.Empty;

        public int RoleId { get; set; } 

        public string RoleName { get; set; } = string.Empty;
    }

}

