using Quiz_API.Entity;

namespace Quiz_API.Models
{
    public class QuizQuestion
    {
       public string QuestionText { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}
