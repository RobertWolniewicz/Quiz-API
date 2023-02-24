using Quiz_API.Entity;

namespace Quiz_API.Models
{
    public class RegisterUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddres { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public int RoleId { get; set; }
        public CompanyDto Company { get; set; }
    }
}
