using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface IQuizServices
    {
        List<Question> GetQuiz (QuizParameters parameters, User user);
        int PostResult( User user, List<AnswersModel> Answers);
    }

    public class QuizServices : IQuizServices
    {
        private readonly AppDB _dbContext;
        public QuizServices(AppDB DbContext)
        {
            _dbContext = DbContext;
        }
        public List<Question> GetQuiz(QuizParameters parameters, User user)
        {
            var Questions = new List<Question>();
            var easyQuestions = GetQuestions<EasyQuestion>(parameters.Category, parameters.NumberOfEasyQuestions);
            var midQuestions = GetQuestions<MidQuestion>(parameters.Category, parameters.NumberOfMidQuestions);
            var hardQuestions = GetQuestions<HardQuestion>(parameters.Category, parameters.NumberOfHardQuestions);
            foreach (Question question in easyQuestions)
            {
                Questions.Add(question);
                user.QuestionsList.Add (question);
            }
            foreach (Question question in midQuestions)
            {
                Questions.Add(question);
                user.QuestionsList.Add(question);
            }
            foreach (Question question in hardQuestions)
            {
                Questions.Add(question);
                user.QuestionsList.Add(question);
            }

            return Questions;
        }
        public int PostResult(User user,  List<AnswersModel> Answers)
        {
            var result = 0;
            foreach(Question question in user.QuestionsList)
            {
                var questionAnswer = Answers.First(a => a.QuestionId == question.Id);
                    if(question is EasyQuestion question1)
                {
                    if (questionAnswer.EasyAnswer==question1.CorrectAnswer)
                    {
                        result += question.Points;
                    }
                }
                    if ((question is HardQuestion)|| (question is MidQuestion))
                        {
                    if (questionAnswer.Answer == question.CorrectAnswer)
                    {
                        result += question.Points;
                    }
                }
            }
            user.QuestionsList.Clear();
            return result;
        }
        List<T> GetQuestions<T>(Category category, int quantyty) where T : Question
        {
            return _dbContext.Set<T>().Include(t => t.Answers).Where(t => t.Categorys.Contains(category)).OrderBy(o => Guid.NewGuid()).Take(quantyty).ToList();
        }
    }
}
