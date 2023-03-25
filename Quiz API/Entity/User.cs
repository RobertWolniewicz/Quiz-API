namespace Quiz_API.Entity
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddres { get; set; }
        public string Password { get; set; }
        public List<Question> QuestionsList { get; set; } = new List<Question>();
        public string QuizCategoryName { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
