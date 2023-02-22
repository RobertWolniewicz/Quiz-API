using Quiz_API.Entity;

namespace Quiz_API.Models
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public List<CategoryDto> Categorys { get; set; } = new List<CategoryDto>();
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public string CorrectAnswer { get; set; }
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }
}
