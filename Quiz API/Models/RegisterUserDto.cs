using Quiz_API.Entity;

namespace Quiz_API.Models
{
    public class RegisterUserDto : UserDto
    {
        public string ConfirmedPassword { get; set; }
        public CompanyDto Company { get; set; }
    }
}
