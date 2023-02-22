namespace Quiz_API.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddres { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
