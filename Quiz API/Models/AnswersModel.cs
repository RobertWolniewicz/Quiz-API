namespace Quiz_API.Models
{
    public class AnswersModel
    {
        public int QuestionId  { get; set; }
        public string Answer { get; set; }
        public bool EasyAnswer { get; set; }
    }
}
