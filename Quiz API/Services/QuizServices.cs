﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Exceptions;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface IQuizServices
    {
        Task<List<QuizQuestion>> GetQuiz(QuizParameters parameters);
        Task<bool> PostResult(List<AnswerDto> Answers);
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
            var user = await _dbContext.Users.Include(u => u.QuestionsList).FirstOrDefaultAsync(u => u.Id == _userContextServices.GetUserId);
            user.QuestionsList.Clear();
            user.QuizCategoryName = null;
            var Questions = new List<QuizQuestion>();
            var easyQuestions = await GetQuestions<EasyQuestion>(parameters.Category, parameters.NumberOfEasyQuestions);
            var midQuestions = await GetQuestions<MidQuestion>(parameters.Category, parameters.NumberOfMidQuestions);
            var hardQuestions = await GetQuestions<HardQuestion>(parameters.Category, parameters.NumberOfHardQuestions);
            foreach (Question question in easyQuestions)
            {
                question.Answers.Add(new()
                {
                    Text = "Yes",
                    QuestionId = question.Id
                });
                question.Answers.Add(new()
                {
                    Text = "No",
                    QuestionId = question.Id
                });
                var quizQuestion = _mapper.Map<QuizQuestion>(question);
                Questions.Add(quizQuestion);
                user.QuestionsList.Add(question);
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
            user.QuizCategoryName = parameters.Category;
            await _dbContext.SaveChangesAsync();
            return Questions;
        }
        public async Task<bool> PostResult(List<AnswerDto> Answers)
        {
            
            var user = await _dbContext.Users.Include(u => u.QuestionsList).FirstOrDefaultAsync(u => u.Id == _userContextServices.GetUserId);
            if (user.QuestionsList.Count == 0)
            {
                throw new NotFoundException("Quiz not found");
            }
            double result = 0;
            double totalPoints = 0;
            bool isPass = false;
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == user.QuizCategoryName);
            foreach (AnswerDto Answer in Answers)
            {
                var question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == Answer.QuestionId);
                totalPoints += question.Points;
                if (Answer.Text == question.CorrectAnswer)
                {
                    result += question.Points;
                }
            }
            if(totalPoints*category.CorrectAnswersPercent<=totalPoints)
            {
                isPass = true;
            }
            user.QuestionsList.Clear();
            user.QuizCategoryName = null;
            await _dbContext.SaveChangesAsync();
            var EmailParameters = await _dbContext.EmailParams.FirstOrDefaultAsync();
            if (EmailParameters == null)
            {
                return isPass;
            }
            var addreses = new List<string>()
            {
                user.EmailAddres
            };
            if (user is CompanyUser)
            {
                var companyUser = user as CompanyUser;
                addreses.Add(companyUser.Company.EmailAddres);
            }
            EmailSender.send(EmailParameters, addreses, result);
            return isPass;
        }
        public async Task<List<QuizQuestion>> GetUserQuiz()
        {
            var oldQuiz = new List<QuizQuestion>();
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == _userContextServices.GetUserId);
            var userQuiz = await _dbContext.Questions.Include(q => q.Answers).Where(q => q.Users.Contains(user)).ToListAsync();
            if (userQuiz == null)
            {
                throw new NotFoundException("Quiz not found");
            }
            foreach (Question question in userQuiz)
            {
                var quizQuestion = _mapper.Map<QuizQuestion>(question);
                oldQuiz.Add(quizQuestion);

            }
            return oldQuiz;
        }
        async Task<List<T>> GetQuestions<T>(string category, int quantyty) where T : Question
        {
            var chosenCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == category);
            return await _dbContext.Set<T>().Include(t => t.Answers).Where(t => t.Categorys.Contains(chosenCategory)).OrderBy(o => Guid.NewGuid()).Take(quantyty).ToListAsync();
        }
    }
}
