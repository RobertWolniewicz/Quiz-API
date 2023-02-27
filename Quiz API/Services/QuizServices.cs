using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Exceptions;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface IQuizServices
    {
        Task<List<QuizQuestion>> GetQuiz (QuizParameters parameters);
        Task<int> PostResult(List<AnswerDto> Answers);
        Task<List<QuizQuestion>> GetUserQuiz();
    }

    public class QuizServices : IQuizServices
    {
        private readonly AppDB _dbContext;
        private readonly IUserContextServices _userContextServices;
        private readonly IMapper _mapper;

        public QuizServices(AppDB DbContext, IUserContextServices userContextServices, IMapper mapper)
        {
            _dbContext = DbContext;
            _userContextServices = userContextServices;
            _mapper = mapper;
        }
        public async Task<List<QuizQuestion>> GetQuiz(QuizParameters parameters)
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == _userContextServices.GetUserId);
            var Questions = new List<QuizQuestion>();
            var easyQuestions = await GetQuestions<EasyQuestion>(parameters.Category, parameters.NumberOfEasyQuestions);
            var midQuestions = await GetQuestions<MidQuestion>(parameters.Category, parameters.NumberOfMidQuestions);
            var hardQuestions = await GetQuestions<HardQuestion>(parameters.Category, parameters.NumberOfHardQuestions);
            foreach (Question question in easyQuestions)
            {
                var quizQuestion=  _mapper.Map<QuizQuestion>(question);
                Questions.Add(quizQuestion);
                user.QuestionsList.Add (question);
            }
            foreach (Question question in midQuestions)
            {
                var quizQuestion = _mapper.Map<QuizQuestion>(question);
                Questions.Add(quizQuestion);
                user.QuestionsList.Add(question);
            }
            foreach (Question question in hardQuestions)
            {
                var quizQuestion = _mapper.Map<QuizQuestion>(question);
                Questions.Add(quizQuestion);
                user.QuestionsList.Add(question);
            }

            return Questions;
        }
        public async Task<int> PostResult(List<AnswerDto> Answers)
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == _userContextServices.GetUserId);
            var result = 0;
            foreach(AnswerDto Answer in Answers)
            {
                var dbAnswer = await _dbContext.answers.FirstOrDefaultAsync(a =>a.Id==Answer.Id);
                var question = await _dbContext.questions.FirstOrDefaultAsync(q => q.Id== dbAnswer.QuestionId);
                    if (Answer.Text == question.CorrectAnswer)
                    {
                        result += question.Points;
                    }
            }
            user.QuestionsList.Clear();
            return result;
        }
        public async Task<List<QuizQuestion>> GetUserQuiz()
        {
            var oldQuiz = new List<QuizQuestion>();
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == _userContextServices.GetUserId);
            if(user.QuestionsList==null)
            {
                throw new NotFoundException("QuizAPI not found");
            }
            foreach (Question question in user.QuestionsList)
            {
                var quizQuestion = _mapper.Map<QuizQuestion>(question);
                oldQuiz.Add(quizQuestion);

            }
            return oldQuiz;
        }
        async Task<List<T>> GetQuestions<T>(Category category, int quantyty) where T : Question
        {
           return await _dbContext.Set<T>().Include(t => t.Answers).Where(t => t.Categorys.Contains(category)).OrderBy(o => Guid.NewGuid()).Take(quantyty).ToListAsync();
            
        }
    }
}
