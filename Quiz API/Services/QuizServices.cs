using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface IQuizServices
    {
        List<Question> GetQuiz(Category category, QuizParameters parameters, User user);
        int PostResult( User user, List<AnswersModel> Answers);
    }

    public class QuizServices : IQuizServices
    {
        private readonly AppDB _dbContext;
        public QuizServices(AppDB DbContext)
        {
            _dbContext = DbContext;
        }
        public List<Question> GetQuiz(Category category, QuizParameters parameters, User user)
        {
            var Questions = new List<Question>();
            var easyQuestions = GetQuestions<EasyQuestion>(category, parameters.numberOfEasyQuestions);
            var midQuestions = GetQuestions<MidQuestion>(category, parameters.numberOfMidQuestions);
            var hardQuestions = GetQuestions<HardQuestion>(category, parameters.numberOfHardQuestions);
            foreach (Question question in easyQuestions)
            {
                Questions.Add(question);
                user.questionsList.Add (question);
            }
            foreach (Question question in midQuestions)
            {
                Questions.Add(question);
                user.questionsList.Add(question);
            }
            foreach (Question question in hardQuestions)
            {
                Questions.Add(question);
                user.questionsList.Add(question);
            }

            return Questions;
        }
        public int PostResult(User user,  List<AnswersModel> Answers)
        {
            var result = 0;
            foreach(Question question in user.questionsList)
            {
                var questionAnswer = Answers.First(a => a.QuestionId == question.Id);
                    if(question is EasyQuestion)
                {
                    if (questionAnswer.EasyAnswer==((EasyQuestion)question).CorrectAnswer)
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
            return result;
        }
        List<T> GetQuestions<T>(Category category, int quantyty) where T : Question
        {
            return _dbContext.Set<T>().Include(t => t.Answers).Where(t => t.Categorys.Contains(category)).OrderBy(o => Guid.NewGuid()).Take(quantyty).ToList();
        }
    }
}
