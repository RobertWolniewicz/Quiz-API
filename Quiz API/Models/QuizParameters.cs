using Quiz_API.Entity;

namespace Quiz_API.Models
{
    public class QuizParameters
    {
        public string Category { get; set; }
        public int NumberOfEasyQuestions { get; set; }
        public int NumberOfMidQuestions { get; set; }
        public int NumberOfHardQuestions { get; set; }
    }
}
