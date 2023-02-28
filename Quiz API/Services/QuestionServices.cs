using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Exceptions;
using Quiz_API.Models;
using Sieve.Models;
using Sieve.Services;

namespace Quiz_API.Services
{
    public interface IQuestionServices
    {
        Task<QuestionDto> Create<T>(NewQuestionModel newQuestion) where T : Question, new ();
        Task Delete(int id);
        Task<PageResult<QuestionDto>> GetAll(SieveModel query, ISieveProcessor sieveprocessor);
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
        public async Task<PageResult<QuestionDto>> GetAll(SieveModel query, ISieveProcessor sieveProcessor)
        {
            var questionsQuery =  _dbContext.questions.Include(q => q.Answers).Include(q => q.Categorys).AsQueryable();
            var Questions = await sieveProcessor.Apply(query, questionsQuery).ToListAsync();
            var QuestionDtos = _mapper.Map<List<QuestionDto>>(Questions);
            var totalCount = await sieveProcessor.Apply(query, questionsQuery, applyPagination: false, applySorting: false).CountAsync();
            return new PageResult<QuestionDto>(QuestionDtos, totalCount, query.PageSize.Value, query.Page.Value);
        }
        public async Task<QuestionDto> GetById(int id) 
        {
            var Question = await FindById(id);
            var QuestionDto = _mapper.Map<QuestionDto>(Question);
            return QuestionDto;
        }
        public async Task Delete(int id) 
        {
            var Question = await FindById(id);
            _dbContext.questions.Remove(Question);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Update(QuestionDto updatingQuestion)
        {
            var existingQuestion = await FindById(updatingQuestion.Id);
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
            await _dbContext.SaveChangesAsync();
        }
        public async Task<QuestionDto> Create<T>(NewQuestionModel newQuestion) where T : Question, new()
        {
           T createdQuestion = new()
            {
                QuestionText = newQuestion.QuestionText,
            };
           foreach(var category in newQuestion.Categorys)
            {
                var Category=await _dbContext.categories.FirstOrDefaultAsync(c => c.Name == category);
                if (Category == null)
                {
                    Category = new()
                    {
                        Name = category,
                    };
                }
                createdQuestion.Categorys.Add(Category);
            }
                createdQuestion.CorrectAnswer = newQuestion.CorrectAnswer;
                foreach (var answer in newQuestion.Answers)
                {
                    var newAnswer = new Answer()
                    {
                        Text = answer
                    };
                    createdQuestion.Answers.Add(newAnswer);
                }
                _dbContext.questions.Add(createdQuestion);
            await _dbContext.SaveChangesAsync();
            return  _mapper.Map<QuestionDto>(createdQuestion);
        }
        async Task<Question> FindById(int Id)
        {
            var result = await _dbContext.questions.Include(q => q.Answers).Include(q => q.Categorys).FirstOrDefaultAsync(q => q.Id == Id);
            if (result == null)
            {
                throw new NotFoundException("Question not found");
            }
            return result;
        }
    }
}
