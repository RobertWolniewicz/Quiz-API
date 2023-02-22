using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface IQuestionServices
    {
        QuestionDto Create<T>(QuestionDto newQuestion) where T : Question, new ();
        void Delete(int id);
        List<QuestionDto> GetAll();
        QuestionDto GetById(int id);
        void Update(QuestionDto updatingQuestion);
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
        public List<QuestionDto> GetAll()
        {
            var Questions = _dbContext.questions.Include(q => q.Answers).Include(q => q.Categorys);
            var QuestionDtos = _mapper.Map<List<QuestionDto>>(Questions);
            return QuestionDtos;
        }
        public QuestionDto GetById(int id) 
        {
            var Question = _dbContext.questions.FirstOrDefault(q => q.Id == id);
            var QuestionDto = _mapper.Map<QuestionDto>(Question);
            return QuestionDto;
        }
        public void Delete(int id) 
        {
            _dbContext.questions.Remove(_dbContext.questions.FirstOrDefault(q => q.Id == id));
            _dbContext.SaveChanges();
        }
        public void Update(QuestionDto updatingQuestion)
        {
            var existingQuestion = GetById(updatingQuestion.Id);
            if (existingQuestion == null) return;
            existingQuestion.QuestionText = updatingQuestion.QuestionText;
            existingQuestion.CorrectAnswer = updatingQuestion.CorrectAnswer;
            existingQuestion.Categorys = updatingQuestion.Categorys;
            existingQuestion.Answers = updatingQuestion.Answers;
            _dbContext.SaveChanges();
        }
        public QuestionDto Create<T>(QuestionDto newQuestion) where T : Question, new()
        {
            if(!(_dbContext.Set<T>().FirstOrDefault(q=>q.QuestionText == newQuestion.QuestionText)==null))
            {
                return null;
            }
           T createdQuestion = new()
            {
                QuestionText = newQuestion.QuestionText,
            };
           foreach(var category in newQuestion.Categorys)
            {
                var Category=_dbContext.categories.FirstOrDefault(c => c.Name == category.Name);
                if (Category == null)
                {
                    Category = new()
                    {
                        Name = category.Name,
                    };
                    _dbContext.categories.Add(Category);
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
                _dbContext.SaveChanges();
            return  _mapper.Map<QuestionDto>(createdQuestion);
        }
    }
}
