namespace VisitorRequest.Dto
{
    public class AppUser
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string LoginPassword { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
