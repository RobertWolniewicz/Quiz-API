namespace Quiz_API.Models
{
    public class NewQuestionModel
    {
        public List<string> Categorys { get; set; } = new List<string>();
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> Answers { get; set; } = new List<string>();
    }
}
