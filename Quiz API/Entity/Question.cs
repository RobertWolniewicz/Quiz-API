using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_API.Entity
{
    public abstract class Question
    {
        public int Id { get; set; }
        public List<Category> Categorys { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        [NotMapped]
        public virtual object CorrectAnswer { get; set; }
        public List<User> Users { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
