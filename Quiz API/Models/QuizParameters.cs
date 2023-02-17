using Quiz_API.Entity;

namespace Quiz_API.Models
{
    public class QuizParameters
    {
        public int numberOfEasyQuestions { get; set; }
        public int numberOfMidQuestions { get; set; }
        public int numberOfHardQuestions { get; set; }
        public User user { get; set; }
    }
}
