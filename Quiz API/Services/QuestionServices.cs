using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Exceptions;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface IQuestionServices
    {
        Task<QuestionDto> Create<T>(QuestionDto newQuestion) where T : Question, new ();
        Task Delete(int id);
        Task<List<QuestionDto>> GetAll();
        Task<QuestionDto> GetById(int id);
        Task Update(QuestionDto updatingQuestion);
    }

    public class QuestionServices : IQuestionServices
    {
        private readonly AppDB _dbContext;
        private readonly IMapper _mapper;

        public QuestionServices(AppDB DbContext, IMapper mapper)
        {
            _dbContext = DbContext;
            _mapper = mapper;
        }
        public async Task<List<QuestionDto>> GetAll()
        {
            var Questions = await _dbContext.questions.Include(q => q.Answers).Include(q => q.Categorys).ToListAsync();
            var QuestionDtos = _mapper.Map<List<QuestionDto>>(Questions);
            return QuestionDtos;
        }
        public async Task<QuestionDto> GetById(int id) 
        {
            var Question = await _dbContext.questions.FirstOrDefaultAsync(q => q.Id == id);
            if(Question == null)
            {
                throw new NotFoundException("Question not found");
            }
            var QuestionDto = _mapper.Map<QuestionDto>(Question);
            return QuestionDto;
        }
        public async Task Delete(int id) 
        {
            var Question = await _dbContext.questions.FirstOrDefaultAsync(q => q.Id == id);
            if (Question == null)
            {
                throw new NotFoundException("Question not found");
            }
            _dbContext.questions.Remove(Question);
            _dbContext.SaveChangesAsync();
        }
        public async Task Update(QuestionDto updatingQuestion)
        {
            var existingQuestion = await _dbContext.questions.FirstOrDefaultAsync(q => q.Id == updatingQuestion.Id);
            if (existingQuestion == null)
            {
                throw new NotFoundException("Question not found");
            }
            existingQuestion.QuestionText = updatingQuestion.QuestionText;
            existingQuestion.CorrectAnswer = updatingQuestion.CorrectAnswer;
            _dbContext.RemoveRange(existingQuestion.Categorys);
            _dbContext.RemoveRange(existingQuestion.Answers);
            foreach (var category in updatingQuestion.Categorys)
            {
                var Category = await _dbContext.categories.FirstOrDefaultAsync(c => c.Name == category.Name);
                if (Category == null)
                {
                    Category = new()
                    {
                        Name = category.Name,
                    };
                }
                existingQuestion.Categorys.Add(Category);
            }
            foreach (var answer in updatingQuestion.Answers)
            {
                var newAnswer = new Answer()
                {
                    Text = answer.Text
                };
                existingQuestion.Answers.Add(newAnswer);
            }
            _dbContext.SaveChangesAsync();
        }
        public async Task<QuestionDto> Create<T>(QuestionDto newQuestion) where T : Question, new()
        {
           T createdQuestion = new()
            {
                QuestionText = newQuestion.QuestionText,
            };
           foreach(var category in newQuestion.Categorys)
            {
                var Category=await _dbContext.categories.FirstOrDefaultAsync(c => c.Name == category.Name);
                if (Category == null)
                {
                    Category = new()
                    {
                        Name = category.Name,
                    };
                }
                createdQuestion.Categorys.Add(Category);
            }
                createdQuestion.CorrectAnswer = newQuestion.CorrectAnswer;
                foreach (var answer in newQuestion.Answers)
                {
                    var newAnswer = new Answer()
                    {
                        Text = answer.Text
                    };
                    createdQuestion.Answers.Add(newAnswer);
                }
                _dbContext.questions.Add(createdQuestion);
                _dbContext.SaveChangesAsync();
            return  _mapper.Map<QuestionDto>(createdQuestion);
        }
    }
}
